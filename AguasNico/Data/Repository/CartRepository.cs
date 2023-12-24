using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;

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
    }
}