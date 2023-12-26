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
    public class DispatchedProductRepository(ApplicationDbContext db) : Repository<DispatchedProduct>(db), IDispatchedProductRepository
    {
        private readonly ApplicationDbContext _db = db;

        public void Update(DispatchedProduct product)
        {
            var dbObject = _db.DispatchedProducts.First(x => x.Type == product.Type && x.RouteID == product.RouteID) ?? throw new Exception("No se ha encontrado el producto");
            dbObject.Quantity = product.Quantity;
            dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            _db.SaveChanges();
        }

        public void SoftDeleteAll(long routeID)
        {
            List<DispatchedProduct> products = _db.DispatchedProducts.Where(x => x.RouteID == routeID).ToList();
            foreach (DispatchedProduct product in products)
            {
                product.DeletedAt = DateTime.UtcNow.AddHours(-3);
            }
            _db.SaveChanges();
        }

        public IEnumerable<DispatchedProduct> GetAllFromRoute(long routeID)
        {
            return _db.DispatchedProducts.Where(x => x.RouteID == routeID);
        }
    }
}