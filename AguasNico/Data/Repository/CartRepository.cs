using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using Microsoft.EntityFrameworkCore;

namespace AguasNico.Data.Repository
{
    public class CartRepository(ApplicationDbContext db) : Repository<Cart>(db), ICartRepository
    {
        private readonly ApplicationDbContext _db = db;

        public void UpdateProducts(long cartID, List<CartProduct> products)
        {
            // TODO: Implement this
        }

        public void SoftDelete(long id)
        {
            try
            {
                bool isInTransaction = _db.Database.CurrentTransaction is not null;
                if (!isInTransaction) _db.Database.BeginTransaction();
                Cart cart = _db.Carts
                    .Where(x => x.ID == id)
                    .Include(x => x.Client)
                    .Include(x => x.Products)
                        .ThenInclude(x => x.Product)
                    .Include(x => x.ReturnedProducts)
                        .ThenInclude(x => x.Product)
                    .Include(x => x.PaymentMethods)
                    .FirstOrDefault() ?? throw new Exception("No se ha encontrado la bajada");

                if (cart.Products is not null)
                {
                    foreach (CartProduct product in cart.Products)
                    {
                        ClientProduct clientProduct = _db.ClientProducts.Where(x => x.ClientID == cart.ClientID && x.ProductID == product.ProductID).FirstOrDefault() ?? throw new Exception("No se ha encontrado un producto del cliente");
                        clientProduct.Stock -= product.Quantity;
                        cart.Client.Debt -= product.Quantity * product.SettedPrice;
                        product.DeletedAt = DateTime.UtcNow.AddHours(-3);
                        _db.SaveChanges();
                    }
                }

                if (cart.ReturnedProducts is not null)
                {
                    foreach (ReturnedProduct product in cart.ReturnedProducts)
                    {
                        ClientProduct clientProduct = _db.ClientProducts.Where(x => x.ClientID == cart.ClientID && x.ProductID == product.ProductID).FirstOrDefault() ?? throw new Exception("No se ha encontrado un producto del cliente");
                        clientProduct.Stock += product.Quantity;
                        product.DeletedAt = DateTime.UtcNow.AddHours(-3);
                        _db.SaveChanges();
                    }
                }

                if (cart.PaymentMethods is not null)
                {
                    foreach (CartPaymentMethod paymentMethod in cart.PaymentMethods)
                    {
                        cart.Client.Debt += paymentMethod.Amount;
                        paymentMethod.DeletedAt = DateTime.UtcNow.AddHours(-3);
                        _db.SaveChanges();
                    }
                }

                cart.DeletedAt = DateTime.UtcNow.AddHours(-3);

                _db.SaveChanges();
                if (!isInTransaction) _db.Database.CommitTransaction();
            }
            catch (Exception)
            {
                _db.Database.RollbackTransaction();
                throw;
            }
        }

        public IEnumerable<Cart> GetLastTen(long clientID)
        {
            return _db.Carts
                .Where(x => x.ClientID == clientID && !x.IsStatic)
                .OrderByDescending(x => x.CreatedAt)
                .Take(10)
                .Include(x => x.Products)
                    .ThenInclude(x => x.Product)
                .Include(x => x.PaymentMethods)
                    .ThenInclude(x => x.PaymentMethod);
        }

        public void Update(Cart cart)
        {
            _db.Carts.Update(cart);
        }

        public void Confirm(Cart cart)
        {
            try
            {
                _db.Database.BeginTransaction();
                Client client = _db.Clients.Find(cart.ClientID) ?? throw new Exception("No se ha encontrado el cliente");

                decimal total = 0;
                if (cart.Products is not null)
                {
                    foreach (CartProduct product in cart.Products)
                    {
                        if (product.Quantity <= 0) continue;
                        ClientProduct clientProduct = _db.ClientProducts.Where(x => x.ClientID == cart.ClientID && x.ProductID == product.ProductID).Include(x => x.Product).FirstOrDefault() ?? throw new Exception("No se ha encontrado un producto del cliente");
                        clientProduct.Stock += product.Quantity;
                        product.SettedPrice = clientProduct.Product.Price;
                        product.CartID = cart.ID;
                        total += product.Quantity * product.SettedPrice;
                        _db.CartProducts.Add(product);
                    }
                    client.Debt += total;
                }

                total = 0;
                if (cart.PaymentMethods is not null)
                {
                    foreach (CartPaymentMethod paymentMethod in cart.PaymentMethods)
                    {
                        paymentMethod.CartID = cart.ID;
                        total += paymentMethod.Amount;
                        _db.CartPaymentMethods.Add(paymentMethod);
                    }
                    client.Debt -= total;
                }

                Cart updatedCart = _db.Carts.Find(cart.ID) ?? throw new Exception("No se ha encontrado la bajada");
                updatedCart.State = State.Confirmed;
                _db.SaveChanges();
                _db.Database.CommitTransaction();
            }
            catch (Exception)
            {
                _db.Database.RollbackTransaction();
                throw;
            }
        }

        public void CreateManual(Cart cart)
        {
            try
            {
                _db.Database.BeginTransaction();
                cart.State = State.Confirmed;
                cart.IsStatic = false;
                _db.Carts.Add(cart);
                _db.SaveChanges();

                Client client = _db.Clients.Find(cart.ClientID) ?? throw new Exception("No se ha encontrado el cliente");

                decimal total = 0;
                if (cart.Products is not null)
                {
                    foreach (CartProduct product in cart.Products)
                    {
                        if (product.Quantity <= 0) continue;
                        ClientProduct clientProduct = _db.ClientProducts.Where(x => x.ClientID == cart.ClientID && x.ProductID == product.ProductID).Include(x => x.Product).FirstOrDefault() ?? throw new Exception("No se ha encontrado un producto del cliente");
                        clientProduct.Stock += product.Quantity;
                        product.SettedPrice = clientProduct.Product.Price;
                        product.CartID = cart.ID;
                        total += product.Quantity * product.SettedPrice;
                    }
                    client.Debt += total;
                }

                total = 0;
                if (cart.PaymentMethods is not null)
                {
                    foreach (CartPaymentMethod paymentMethod in cart.PaymentMethods)
                    {
                        paymentMethod.CartID = cart.ID;
                        total += paymentMethod.Amount;
                    }
                    client.Debt -= total;
                }

                _db.SaveChanges();
                _db.Database.CommitTransaction();
            }
            catch (Exception)
            {
                _db.Database.RollbackTransaction();
                throw;
            }
        }

        public List<ReturnedProduct> GetReturnedProducts(long cartID)
        {
            Cart cart = _db.Carts.Find(cartID) ?? throw new Exception("No se ha encontrado la bajada");
            List<ReturnedProduct> returnedProducts = _db.ReturnedProducts.Where(x => x.CartID == cartID).Include(x => x.Product).ToList();
            List<ReturnedProduct> products = [];
            foreach (ClientProduct product in _db.ClientProducts.Where(x => x.ClientID == cart.ClientID).Include(x => x.Product))
            {
                if (returnedProducts.Any(x => x.Product.Type == product.Product.Type))
                {
                    products.Add(returnedProducts.First(x => x.Product.Type == product.Product.Type));
                }
                else
                {
                    products.Add(new()
                    {
                        ProductID = product.ProductID,
                        Product = product.Product,
                        Quantity = 0,
                    });
                }
            }
            return products;
        }

        public void ReturnProducts(long cartID, List<ReturnedProduct> products)
        {
            try
            {
                _db.Database.BeginTransaction();
                Cart cart = _db.Carts.Include(x => x.ReturnedProducts).Where(x => x.ID == cartID).First() ?? throw new Exception("No se ha encontrado la bajada");
                Client client = _db.Clients.Include(x => x.Products).ThenInclude(x => x.Product).Where(x => x.ID == cart.ClientID).First() ?? throw new Exception("No se ha encontrado el cliente");
                
                foreach (ReturnedProduct product in cart.ReturnedProducts)
                {
                    ClientProduct clientProduct = client.Products.First(x => x.ProductID == product.ProductID);
                    clientProduct.Stock += product.Quantity;
                    product.DeletedAt = DateTime.UtcNow.AddHours(-3);
                    _db.SaveChanges();
                }

                foreach (ReturnedProduct product in products)
                {
                    if (product.Quantity <= 0) continue;
                    ClientProduct clientProduct = client.Products.First(x => x.Product.Type == product.Product.Type);
                    if (clientProduct.Stock < product.Quantity) throw new Exception("El cliente no posee stock suficiente de: " + product.Product.Type.GetDisplayName());
                    clientProduct.Stock -= product.Quantity;

                    //Ignorar los filtros globales
                    ReturnedProduct? existingReturnedProducts = _db.ReturnedProducts.IgnoreQueryFilters().Where(x => x.CartID == cart.ID && x.Product.Type == product.Product.Type).FirstOrDefault();
                    if (existingReturnedProducts is not null)
                    {
                        existingReturnedProducts.Quantity = product.Quantity;
                        existingReturnedProducts.UpdatedAt = DateTime.UtcNow.AddHours(-3);
                        existingReturnedProducts.DeletedAt = null;
                        _db.SaveChanges();
                    }
                    else
                    {
                        _db.ReturnedProducts.Add(new()
                        {
                            CartID = cart.ID,
                            ProductID = clientProduct.ProductID,
                            Quantity = product.Quantity,
                        });
                    }
                }
                
                _db.SaveChanges();
                _db.Database.CommitTransaction();
            }
            catch (Exception)
            {
                _db.Database.RollbackTransaction();
                throw;
            }
        }
    }
}