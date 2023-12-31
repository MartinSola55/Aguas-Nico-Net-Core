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
        void Update(Cart cart);
        void UpdateProducts(long cartID, List<CartProduct> products);
        void SoftDelete(long id);
        IEnumerable<Cart> GetLastTen(long clientID);
        void Confirm(Cart cart);
        void CreateManual(Cart cart);
        List<ReturnedProduct> GetReturnedProducts(long cartID);
        void ReturnProducts(long cartID, List<ReturnedProduct> products);
    }
}