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

        public void Update(Cart cart)
        {
            try
            {
                _db.Database.BeginTransaction();
                
                this.SoftDelete(cart.ID);
                Client client = _db.Clients.Include(x => x.Products).ThenInclude(x => x.Product).Where(x => x.ID == cart.ClientID).First() ?? throw new Exception("No se ha encontrado el cliente");

                if (cart.Products is not null)
                {
                    foreach(CartProduct product in cart.Products)
                    {
                        if (product.Quantity <= 0) continue;
                        ClientProduct clientProduct = client.Products.First(x => x.Product.Type == product.Type);
                        clientProduct.Stock += product.Quantity;
                        client.Debt += product.Quantity * clientProduct.Product.Price;

                        //Ignorar los filtros globales
                        CartProduct? existingProduct = _db.CartProducts.IgnoreQueryFilters().Where(x => x.CartID == cart.ID && x.Type == product.Type).FirstOrDefault();
                        if (existingProduct is not null)
                        {
                            existingProduct.Quantity = product.Quantity;
                            existingProduct.SettedPrice = clientProduct.Product.Price;
                            existingProduct.UpdatedAt = DateTime.UtcNow.AddHours(-3);
                            existingProduct.DeletedAt = null;
                            _db.SaveChanges();
                        }
                        else
                        {
                            _db.CartProducts.Add(new()
                            {
                                CartID = cart.ID,
                                Type = product.Type,
                                Quantity = product.Quantity,
                                SettedPrice = clientProduct.Product.Price,
                            });
                        }
                    }
                }

                if (cart.AbonoProducts is not null)
                {
                    foreach (CartAbonoProduct product in cart.AbonoProducts)
                    {
                        if (product.Quantity <= 0) continue;
                        ClientProduct clientProduct = client.Products.First(x => x.Product.Type == product.Type);
                        clientProduct.Stock += product.Quantity;

                        List<AbonoRenewalProduct> abonoProducts =
                        [
                            .. _db.AbonoRenewalProducts.Where(x =>
                                x.AbonoRenewal.ClientID == cart.ClientID &&
                                x.CreatedAt.Month == cart.CreatedAt.Month &&
                                x.CreatedAt.Year == cart.CreatedAt.Year &&
                                x.Type == product.Type),
                        ];

                        if (abonoProducts.Sum(x => x.Available) < product.Quantity) throw new Exception("El cliente no posee suficientes " + product.Type.GetDisplayName() + " disponibles en sus abonos");

                        int quantity = product.Quantity;
                        foreach (AbonoRenewalProduct abonoProduct in abonoProducts)
                        {
                            if (abonoProduct.Available >= quantity)
                            {
                                abonoProduct.Available -= quantity;
                                break;
                            }
                            else
                            {
                                quantity -= abonoProduct.Available;
                                abonoProduct.Available = 0;
                            }
                        }

                        //Ignorar los filtros globales
                        CartAbonoProduct? existingProduct = _db.CartAbonoProducts.IgnoreQueryFilters().Where(x => x.CartID == cart.ID && x.Type == product.Type).FirstOrDefault();
                        if (existingProduct is not null)
                        {
                            existingProduct.Quantity = product.Quantity;
                            existingProduct.UpdatedAt = DateTime.UtcNow.AddHours(-3);
                            existingProduct.DeletedAt = null;
                            _db.SaveChanges();
                        }
                        else
                        {
                            _db.CartAbonoProducts.Add(new()
                            {
                                CartID = cart.ID,
                                Type = product.Type,
                                Quantity = product.Quantity,
                            });
                        }
                    }
                }

                if (cart.ReturnedProducts is not null)
                {
                    foreach (ReturnedProduct product in cart.ReturnedProducts)
                    {
                        ClientProduct clientProduct = client.Products.First(x => x.Product.Type == product.Type);
                        if (clientProduct.Stock < product.Quantity) throw new Exception("El cliente no posee stock suficiente de: " + product.Type.GetDisplayName());
                        clientProduct.Stock -= product.Quantity;

                        //Ignorar los filtros globales
                        ReturnedProduct? existingProduct = _db.ReturnedProducts.IgnoreQueryFilters().Where(x => x.CartID == cart.ID && x.Type == product.Type).FirstOrDefault();
                        if (existingProduct is not null)
                        {
                            existingProduct.Quantity = product.Quantity;
                            existingProduct.UpdatedAt = DateTime.UtcNow.AddHours(-3);
                            existingProduct.DeletedAt = null;
                            _db.SaveChanges();
                        }
                        else
                        {
                            _db.ReturnedProducts.Add(new()
                            {
                                CartID = cart.ID,
                                Type = product.Type,
                                Quantity = product.Quantity,
                            });
                        }
                    }
                }

                if (cart.PaymentMethods is not null)
                {
                    foreach (CartPaymentMethod paymentMethod in cart.PaymentMethods)
                    {
                        client.Debt -= paymentMethod.Amount;

                        //Ignorar los filtros globales
                        CartPaymentMethod? existingMethod = _db.CartPaymentMethods.IgnoreQueryFilters().Where(x => x.CartID == cart.ID && x.PaymentMethodID == paymentMethod.PaymentMethodID).FirstOrDefault();
                        if (existingMethod is not null)
                        {
                            existingMethod.Amount = paymentMethod.Amount;
                            existingMethod.UpdatedAt = DateTime.UtcNow.AddHours(-3);
                            existingMethod.DeletedAt = null;
                            _db.SaveChanges();
                        }
                        else
                        {
                            _db.CartPaymentMethods.Add(new()
                            {
                                CartID = cart.ID,
                                PaymentMethodID = paymentMethod.PaymentMethodID,
                                Amount = paymentMethod.Amount,
                            });
                        }
                    }
                }

                Cart updatedCart = _db.Carts.Find(cart.ID) ?? throw new Exception("No se ha encontrado la bajada");
                updatedCart.DeletedAt = null;
                _db.SaveChanges();
                _db.Database.CommitTransaction();
            }
            catch (Exception)
            {
                _db.Database.RollbackTransaction();
                throw;
            }
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
                    .Include(x => x.AbonoProducts)
                    .Include(x => x.ReturnedProducts)
                    .Include(x => x.PaymentMethods)
                    .FirstOrDefault() ?? throw new Exception("No se ha encontrado la bajada");

                if (cart.Products is not null)
                {
                    foreach (CartProduct product in cart.Products)
                    {
                        ClientProduct clientProduct = _db.ClientProducts.Where(x => x.ClientID == cart.ClientID && x.Product.Type == product.Type).FirstOrDefault() ?? throw new Exception("No se ha encontrado un producto del cliente");
                        clientProduct.Stock -= product.Quantity;
                        cart.Client.Debt -= product.Quantity * product.SettedPrice;
                        product.DeletedAt = DateTime.UtcNow.AddHours(-3);
                        _db.SaveChanges();
                    }
                }

                if (cart.AbonoProducts is not null)
                {
                    foreach (CartAbonoProduct abonoProductInCart in cart.AbonoProducts)
                    {
                        AbonoRenewalProduct abonoProduct = _db.AbonoRenewalProducts.Where(x =>
                            x.AbonoRenewal.ClientID == cart.ClientID &&
                            x.CreatedAt.Month == cart.CreatedAt.Month &&
                            x.CreatedAt.Year == cart.CreatedAt.Year &&
                            x.Type == abonoProductInCart.Type).FirstOrDefault() ?? throw new Exception("No se ha encontrado un producto del abono del cliente");

                        abonoProduct.Available += abonoProductInCart.Quantity;

                        ClientProduct clientProduct = _db.ClientProducts.Where(x => x.ClientID == cart.ClientID && x.Product.Type == abonoProductInCart.Type).FirstOrDefault() ?? throw new Exception("No se ha encontrado un producto del cliente");
                        clientProduct.Stock -= abonoProductInCart.Quantity;
                        abonoProductInCart.DeletedAt = DateTime.UtcNow.AddHours(-3);
                        _db.SaveChanges();
                    }
                }

                if (cart.ReturnedProducts is not null)
                {
                    foreach (ReturnedProduct product in cart.ReturnedProducts)
                    {
                        ClientProduct clientProduct = _db.ClientProducts.Where(x => x.ClientID == cart.ClientID && x.Product.Type == product.Type).FirstOrDefault() ?? throw new Exception("No se ha encontrado un producto del cliente");
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
                .Where(x => x.ClientID == clientID && !x.IsStatic && x.State != State.Pending)
                .OrderByDescending(x => x.CreatedAt)
                .Take(10)
                .Include(x => x.Products)
                .Include(x => x.PaymentMethods)
                    .ThenInclude(x => x.PaymentMethod);
        }

        public void Confirm(Cart cart)
        {
            try
            {
                _db.Database.BeginTransaction();
                DateTime today = DateTime.UtcNow.AddHours(-3);
                Client client = _db.Clients.Find(cart.ClientID) ?? throw new Exception("No se ha encontrado el cliente");
                List<ReturnedProduct> returnedProducts = [];
                foreach (ProductType type in Enum.GetValues(typeof(ProductType)))
                {
                    returnedProducts.Add(new()
                    {
                        CartID = cart.ID,
                        Type = type,
                        Quantity = 0,
                    });
                }

                decimal total = 0;
                if (cart.Products is not null)
                {
                    foreach (CartProduct product in cart.Products)
                    {
                        if (product.Quantity <= 0) continue;
                        ClientProduct clientProduct = _db.ClientProducts.Where(x => x.ClientID == cart.ClientID && x.Product.Type == product.Type).Include(x => x.Product).FirstOrDefault() ?? throw new Exception("No se ha encontrado un producto del cliente");
                        clientProduct.Stock += product.Quantity;
                        product.SettedPrice = clientProduct.Product.Price;
                        product.CartID = cart.ID;
                        total += product.Quantity * product.SettedPrice;
                        _db.CartProducts.Add(product);
                        if (returnedProducts.Any(x => x.Type == product.Type))
                        {
                            ReturnedProduct returnedProduct = returnedProducts.First(x => x.Type == product.Type);
                            returnedProduct.Quantity += product.Quantity;
                        }
                    }
                    client.Debt += total;
                }

                if (cart.AbonoProducts is not null)
                {
                    foreach (CartAbonoProduct abonoProduct in cart.AbonoProducts)
                    {
                        if (abonoProduct.Quantity <= 0) continue;

                        List<AbonoRenewalProduct> abonoProductsList =
                        [
                            .. _db.AbonoRenewalProducts.Where(x => 
                            x.AbonoRenewal.ClientID == cart.ClientID &&
                            x.CreatedAt.Month == today.Month &&
                            x.CreatedAt.Year == today.Year &&
                            x.Type == abonoProduct.Type)
                        ];

                        if (abonoProductsList.Count == 0) throw new Exception("No se ha encontrado un producto del abono del cliente");
                        if (abonoProductsList.Sum(x => x.Available) < abonoProduct.Quantity) throw new Exception("El cliente no posee stock suficiente de: " + abonoProduct.Type.GetDisplayName());
                        
                        int quantity = abonoProduct.Quantity;
                        foreach (AbonoRenewalProduct abonoProductList in abonoProductsList)
                        {
                            if (abonoProductList.Available >= quantity)
                            {
                                abonoProductList.Available -= quantity;
                                break;
                            }
                            else
                            {
                                quantity -= abonoProductList.Available;
                                abonoProductList.Available = 0;
                            }
                        }

                        ClientProduct clientProduct = _db.ClientProducts.Where(x => x.ClientID == cart.ClientID && x.Product.Type == abonoProduct.Type).Include(x => x.Product).FirstOrDefault() ?? throw new Exception("No se ha encontrado un producto del cliente");
                        clientProduct.Stock += abonoProduct.Quantity;

                        abonoProduct.CartID = cart.ID;
                        _db.CartAbonoProducts.Add(abonoProduct);

                        if (returnedProducts.Any(x => x.Type == abonoProduct.Type))
                        {
                            ReturnedProduct returnedProduct = returnedProducts.First(x => x.Type == abonoProduct.Type);
                            returnedProduct.Quantity += abonoProduct.Quantity;
                        }
                    }
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

                foreach (ReturnedProduct product in returnedProducts)
                {
                    if (product.Quantity <= 0) continue;
                    ClientProduct clientProduct = _db.ClientProducts.Where(x => x.ClientID == cart.ClientID && x.Product.Type == product.Type).FirstOrDefault() ?? throw new Exception("No se ha encontrado un producto del cliente");
                    clientProduct.Stock -= product.Quantity;
                    _db.ReturnedProducts.Add(product);
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

                DateTime today = DateTime.UtcNow.AddHours(-3);
                Client client = _db.Clients.Find(cart.ClientID) ?? throw new Exception("No se ha encontrado el cliente");

                List<ReturnedProduct> returnedProducts = [];
                foreach (ProductType type in Enum.GetValues(typeof(ProductType)))
                {
                    returnedProducts.Add(new()
                    {
                        CartID = cart.ID,
                        Type = type,
                        Quantity = 0,
                    });
                }

                decimal total = 0;
                if (cart.Products is not null)
                {
                    foreach (CartProduct product in cart.Products)
                    {
                        if (product.Quantity <= 0) continue;
                        ClientProduct clientProduct = _db.ClientProducts.Where(x => x.ClientID == cart.ClientID && x.Product.Type == product.Type).Include(x => x.Product).FirstOrDefault() ?? throw new Exception("No se ha encontrado un producto del cliente");
                        clientProduct.Stock += product.Quantity;
                        product.SettedPrice = clientProduct.Product.Price;
                        total += product.Quantity * product.SettedPrice;
                        if (returnedProducts.Any(x => x.Type == product.Type))
                        {
                            ReturnedProduct returnedProduct = returnedProducts.First(x => x.Type == product.Type);
                            returnedProduct.Quantity += product.Quantity;
                        }
                    }
                    client.Debt += total;
                }

                if (cart.AbonoProducts is not null)
                {
                    foreach (CartAbonoProduct abonoProduct in cart.AbonoProducts)
                    {
                        if (abonoProduct.Quantity <= 0) continue;

                        List<AbonoRenewalProduct> abonoProductsList =
                        [
                            .. _db.AbonoRenewalProducts.Where(x =>
                            x.AbonoRenewal.ClientID == cart.ClientID &&
                            x.CreatedAt.Month == today.Month &&
                            x.CreatedAt.Year == today.Year &&
                            x.Type == abonoProduct.Type)
                        ];

                        if (abonoProductsList.Count == 0) throw new Exception("No se ha encontrado un producto del abono del cliente");
                        if (abonoProductsList.Sum(x => x.Available) < abonoProduct.Quantity) throw new Exception("El cliente no posee stock suficiente de: " + abonoProduct.Type.GetDisplayName());

                        int quantity = abonoProduct.Quantity;
                        foreach (AbonoRenewalProduct abonoProductList in abonoProductsList)
                        {
                            if (abonoProductList.Available >= quantity)
                            {
                                abonoProductList.Available -= quantity;
                                break;
                            }
                            else
                            {
                                quantity -= abonoProductList.Available;
                                abonoProductList.Available = 0;
                            }
                        }

                        ClientProduct clientProduct = _db.ClientProducts.Where(x => x.ClientID == cart.ClientID && x.Product.Type == abonoProduct.Type).Include(x => x.Product).FirstOrDefault() ?? throw new Exception("No se ha encontrado un producto del cliente");
                        clientProduct.Stock += abonoProduct.Quantity;

                        if (returnedProducts.Any(x => x.Type == abonoProduct.Type))
                        {
                            ReturnedProduct returnedProduct = returnedProducts.First(x => x.Type == abonoProduct.Type);
                            returnedProduct.Quantity += abonoProduct.Quantity;
                        }
                    }
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

                foreach (ReturnedProduct product in returnedProducts)
                {
                    if (product.Quantity <= 0) continue;
                    ClientProduct clientProduct = _db.ClientProducts.Where(x => x.ClientID == cart.ClientID && x.Product.Type == product.Type).FirstOrDefault() ?? throw new Exception("No se ha encontrado un producto del cliente");
                    clientProduct.Stock -= product.Quantity;
                    _db.ReturnedProducts.Add(product);
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
            List<ReturnedProduct> returnedProducts = _db.ReturnedProducts.Where(x => x.CartID == cartID).ToList();
            List<ReturnedProduct> products = [];
            foreach (ClientProduct product in _db.ClientProducts.Where(x => x.ClientID == cart.ClientID).Include(x => x.Product))
            {
                if (returnedProducts.Any(x => x.Type == product.Product.Type))
                {
                    products.Add(returnedProducts.First(x => x.Type == product.Product.Type));
                }
                else
                {
                    products.Add(new()
                    {
                        Type = product.Product.Type,
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
                    ClientProduct clientProduct = client.Products.First(x => x.Product.Type == product.Type);
                    clientProduct.Stock += product.Quantity;
                    product.DeletedAt = DateTime.UtcNow.AddHours(-3);
                    _db.SaveChanges();
                }

                foreach (ReturnedProduct product in products)
                {
                    if (product.Quantity <= 0) continue;
                    ClientProduct clientProduct = client.Products.First(x => x.Product.Type == product.Type);
                    if (clientProduct.Stock < product.Quantity) throw new Exception("El cliente no posee stock suficiente de: " + product.Type.GetDisplayName());
                    clientProduct.Stock -= product.Quantity;

                    //Ignorar los filtros globales
                    ReturnedProduct? existingReturnedProducts = _db.ReturnedProducts.IgnoreQueryFilters().Where(x => x.CartID == cart.ID && x.Type == product.Type).FirstOrDefault();
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
                            Type = clientProduct.Product.Type,
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