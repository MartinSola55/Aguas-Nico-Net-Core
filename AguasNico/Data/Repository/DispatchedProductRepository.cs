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

        public async Task Update(DispatchedProduct product)
        {
            var oldProduct = await _db
                .DispatchedProducts
                .FirstAsync(x => x.Type == product.Type && x.RouteID == product.RouteID) ?? throw new Exception("No se ha encontrado el producto");

            oldProduct.Quantity = product.Quantity;
            oldProduct.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            await _db.SaveChangesAsync();
        }

        public async Task SoftDeleteAll(long routeID)
        {
            var products = await _db
                .DispatchedProducts
                .Where(x => x.RouteID == routeID)
                .ToListAsync();

            foreach (var product in products)
            {
                product.DeletedAt = DateTime.UtcNow.AddHours(-3);
            }
            await _db.SaveChangesAsync();
        }

        public async Task<List<DispatchedProduct>> GetAllFromRoute(long routeID)
        {
            return await _db
                .DispatchedProducts
                .Where(x => x.RouteID == routeID)
                .ToListAsync();
        }
    }
}