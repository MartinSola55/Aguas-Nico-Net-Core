using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Models;

namespace AguasNico.Data.Repository.IRepository
{
    public interface IDispatchedProductRepository : IRepository<DispatchedProduct>
    {
        Task<List<DispatchedProduct>> GetAllFromRoute(long routeID);
        Task Update(DispatchedProduct dispatchedProduct);
        Task SoftDeleteAll(long routeID);
    }
}