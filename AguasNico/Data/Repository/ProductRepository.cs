using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;

namespace AguasNico.Data.Repository
{
    public class ProductRepository(ApplicationDbContext db) : Repository<Product>(db), IProductRepository
    {
        private readonly ApplicationDbContext _db = db;

        public void Update(Product product)
        {
            var dbObject = _db.Products.FirstOrDefault(x => x.ID == product.ID);
            if (dbObject != null)
            {
                dbObject.Name = product.Name;
                dbObject.Price = product.Price;
                dbObject.Bottle = product.Bottle;
                dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);
                _db.SaveChanges();
            }
        }

        public void SoftDelete(long id)
        {
            var dbObject = _db.Products.FirstOrDefault(x => x.ID == id);
            if (dbObject != null)
            {
                dbObject.DeletedAt = DateTime.UtcNow.AddHours(-3);
                _db.SaveChanges();
            }
        }
    }
}