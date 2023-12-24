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
                .Include(x => x.PaymentMethods);
        }

        public void Update(Cart cart)
        {
            _db.Carts.Update(cart);
        }
    }
}