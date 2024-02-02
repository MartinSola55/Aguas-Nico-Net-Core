using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using Microsoft.EntityFrameworkCore;

namespace AguasNico.Data.Repository
{
    public class PaymentMethodRepository(ApplicationDbContext db) : Repository<PaymentMethod>(db), IPaymentMethodRepository
    {
        private readonly ApplicationDbContext _db = db;

        public async Task<List<SelectListItem>> GetDropDownList()
        {
            var dropDown = new List<SelectListItem>
            {
                new() { Value = "", Text = "Seleccione un método de pago", Disabled = true, Selected = true }
            };

            var methods = await _db
                .PaymentMethods
                .OrderBy(x => x.ID)
                .Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.ID.ToString(),
                })
                .ToListAsync();

            return [.. dropDown, .. methods];
        }
        public async Task<List<SelectListItem>> GetFilterDropDownList()
        {
            var dropDown = new List<SelectListItem>
            {
                new() { Value = "", Text = "Por método de pago", Selected = true }
            };
            var methods = await _db
                .PaymentMethods
                .OrderBy(x => x.ID)
                .Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Name,
                })
                .ToListAsync();

            return [.. dropDown, .. methods];
        }
    }
}