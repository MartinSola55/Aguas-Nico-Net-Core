using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Models;
using Microsoft.AspNetCore.Mvc;

namespace AguasNico.Data.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task Update(Product product);
        Task<bool> IsDuplicated(Product product);
        Task SoftDelete(long id);
        Task<List<Client>> GetClients(long productID);
        Task<int[]> GetAnnualSales(ProductType productType, DateTime year);
        Task<int> GetClientStock(long productID);
        Task<decimal> GetTotalSold(ProductType productType, DateTime year);
    }
}