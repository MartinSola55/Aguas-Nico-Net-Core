using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Models;

namespace AguasNico.Data.Repository.IRepository
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task Update(Cart cart);
        Task SoftDelete(long id);
        Task<List<Cart>> GetLastTen(long clientID);
        Task Confirm(Cart cart);
        Task CreateManual(Cart cart);
        Task<List<ReturnedProduct>> GetReturnedProducts(long cartID);
        Task ReturnProducts(long cartID, List<ReturnedProduct> products);
    }
}