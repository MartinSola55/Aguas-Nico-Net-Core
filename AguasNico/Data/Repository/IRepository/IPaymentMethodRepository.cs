using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Models;

namespace AguasNico.Data.Repository.IRepository
{
    public interface IPaymentMethodRepository : IRepository<PaymentMethod>
    {
        Task<List<SelectListItem>> GetDropDownList();
        Task<List<SelectListItem>> GetFilterDropDownList();
    }
}