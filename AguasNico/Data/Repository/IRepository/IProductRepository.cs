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
        void Update(Product product);
        bool IsDuplicated(Product product);
        void SoftDelete(long id);
        IEnumerable<Client> GetClients(long productID);
        int[] GetAnnualSales(ProductType productType, DateTime year);
        int GetClientStock(long productID);
        decimal GetTotalSold(ProductType productType, DateTime year);
    }
}