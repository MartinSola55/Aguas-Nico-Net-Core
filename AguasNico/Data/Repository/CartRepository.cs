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
            // TODO: Implement this
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
    }
}