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
        void UpdateProducts(long cartID, List<CartProduct> products);
        void SoftDelete(long id);
    }
}