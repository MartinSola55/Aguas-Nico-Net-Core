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

        public void Update(Product product)
        {
            var dbObject = _db.Products.FirstOrDefault(x => x.ID == product.ID);
            if (dbObject != null)
            {
                dbObject.Name = product.Name;
                dbObject.Price = product.Price;
                dbObject.Type = product.Type;
                dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);
                _db.SaveChanges();
            }
        }

        public bool IsDuplicated(Product product)
        {
            return _db.Products.Any(x => x.Name == product.Name && x.Price == product.Price && x.ID != product.ID);
        }

        public void SoftDelete(long id)
        {
            var dbObject = _db.Products.FirstOrDefault(x => x.ID == id) ?? throw new Exception("No se ha encontrado el producto");
            dbObject.DeletedAt = DateTime.UtcNow.AddHours(-3);
            _db.SaveChanges();
        }

        public IEnumerable<Client> GetClients(long productID)
        {
            return _db.ClientProducts.Where(x => x.ProductID == productID).Include(x => x.Client).ThenInclude(x => x.Dealer).Select(x => x.Client);
        }

        public List<SelectListItem> GetTypes()
        {
            List<SelectListItem> types =
            [
                new() { Value = "", Text = "Seleccione un tipo de producto", Disabled = true, Selected = true }
            ];
            foreach (ProductType type in Enum.GetValues(typeof(ProductType)))
            {
                types.Add(new SelectListItem { Value = ((int)type).ToString(), Text = type.GetDisplayName() });
            }
            return types;
        }

        public int[] GetAnnualSales(ProductType productType, DateTime year)
        {
            var salesByMonth = _db.CartProducts
                .Where(x => x.Type == productType && x.CreatedAt.Year == year.Year)
                .GroupBy(x => x.CreatedAt.Month)
                .Select(x => new
                {
                    Month = x.Key,
                    Total = x.Sum(x => x.Quantity)
                });
            
            int[] sales = new int[12];

            foreach (var sale in salesByMonth)
            {
                sales[sale.Month - 1] = sale.Total;
            }
            return sales;
        }

        public int GetClientStock(long productID)
        {
            return _db.ClientProducts.Where(x => x.ProductID == productID).Sum(x => x.Stock);
        }

        public decimal GetTotalSold(ProductType productType, DateTime year)
        {
            return _db.CartProducts.Where(x => x.Type == productType && x.CreatedAt.Year == year.Year).Sum(x => x.Quantity * x.SettedPrice);
        }
    }
}