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
        IEnumerable<DispatchedProduct> GetAllFromRoute(long routeID);
        void Update(DispatchedProduct dispatchedProduct);
        void SoftDeleteAll(long routeID);
    }
}