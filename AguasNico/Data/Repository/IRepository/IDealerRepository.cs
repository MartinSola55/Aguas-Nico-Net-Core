using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Models;

namespace AguasNico.Data.Repository.IRepository
{
    public interface IDealerRepository : IRepository<ApplicationUser>
    {
        Task<int> GetTotalCarts(string dealerID, DateTime month);
        Task<int> GetTotalCompletedCarts(string dealerID, DateTime month);
        Task<int> GetTotalPendingCarts(string dealerID, DateTime month);
        Task<decimal> GetTotalCollected(string dealerID, DateTime month);
    }
}