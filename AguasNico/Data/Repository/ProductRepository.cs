using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace AguasNico.Data.Repository
{
    public class ProductRepository(ApplicationDbContext db) : Repository<Product>(db), IProductRepository
    {
        private readonly ApplicationDbContext _db = db;

        public async Task Update(Product product)
        {
            var oldProduct = await _db
                .Products
                .FirstOrDefaultAsync(x => x.ID == product.ID);

            if (oldProduct != null)
            {
                oldProduct.Name = product.Name;
                oldProduct.Price = product.Price;
                oldProduct.Type = product.Type;
                oldProduct.SortOrder = product.SortOrder;
                oldProduct.UpdatedAt = DateTime.UtcNow.AddHours(-3);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<bool> IsDuplicated(Product product)
        {
            return await _db
                .Products
                .AnyAsync(x => x.Name == product.Name && x.Price == product.Price && x.IsActive && x.ID != product.ID);
        }

        public async Task SoftDelete(long id)
        {
            var oldProduct = await _db
                .Products
                .FirstOrDefaultAsync(x => x.ID == id) ?? throw new Exception("No se ha encontrado el producto");

            oldProduct.IsActive = false;

            var clientProducts = await _db
                .ClientProducts
                .Where(x => x.ProductID == id)
                .ToListAsync();

            foreach (var clientProduct in clientProducts)
            {
                clientProduct.DeletedAt = DateTime.UtcNow.AddHours(-3);
            }
            try
            {
                await _db.Database.BeginTransactionAsync();
                await _db.SaveChangesAsync();
                await _db.Database.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _db.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<Client>> GetClients(long productID)
        {
            return await _db
                .ClientProducts
                .Where(x => x.ProductID == productID && x.Client.IsActive)
                .Include(x => x.Client)
                    .ThenInclude(x => x.Dealer)
                .Select(x => x.Client)
                .ToListAsync();
        }

        public async Task<int[]> GetAnnualSales(ProductType productType, DateTime year)
        {
            var salesByMonth = await _db
                .CartProducts
                .Where(x => x.Type == productType && x.CreatedAt.Year == year.Year)
                .GroupBy(x => x.CreatedAt.Month)
                .Select(x => new
                {
                    Month = x.Key,
                    Total = x.Sum(x => x.Quantity)
                })
                .ToListAsync();

            int[] sales = new int[12];

            foreach (var sale in salesByMonth)
            {
                sales[sale.Month - 1] = sale.Total;
            }
            return sales;
        }

        public async Task<int> GetClientStock(long productID)
        {
            return await _db
                .ClientProducts
                .Where(x => x.ProductID == productID && x.Client.IsActive)
                .SumAsync(x => x.Stock);
        }

        public async Task<decimal> GetTotalSold(ProductType productType, DateTime year)
        {
            return await _db
                .CartProducts
                .Where(x => x.Type == productType && x.CreatedAt.Year == year.Year)
                .SumAsync(x => x.Quantity * x.SettedPrice);
        }

        public async Task<JsonResult> GetProductsSold(int year, int month)
        {
            var products = await _db
                .CartProducts
                .Where(x => x.Cart.CreatedAt.Year == year && x.Cart.CreatedAt.Month == month)
                .GroupBy(x => x.Type)
                .Select(x => new
                {
                    Type = x.Key.GetDisplayName(),
                    Quantity = x.Sum(x => x.Quantity)
                })
                .ToListAsync();

            foreach (ProductType type in Enum.GetValues(typeof(ProductType)))
            {
                if (!products.Any(x => x.Type == type.GetDisplayName()))
                {
                    products.Add(new
                    {
                        Type = type.GetDisplayName(),
                        Quantity = 0
                    });
                }
            }
            return new JsonResult(new
            {
                success = true,
                data = products.OrderBy(x => x.Type)
            });
        }
    }
}