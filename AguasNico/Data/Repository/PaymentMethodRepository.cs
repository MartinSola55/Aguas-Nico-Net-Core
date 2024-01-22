using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;

namespace AguasNico.Data.Repository
{
    public class PaymentMethodRepository(ApplicationDbContext db) : Repository<PaymentMethod>(db), IPaymentMethodRepository
    {
        private readonly ApplicationDbContext _db = db;

        public IEnumerable<SelectListItem> GetDropDownList()
        {
            IEnumerable<SelectListItem> methods = new List<SelectListItem>
            {
                new() { Value = "", Text = "Seleccione un método de pago", Disabled = true, Selected = true }
            };
            return methods.Concat(_db.PaymentMethods.OrderBy(x => x.ID).Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.ID.ToString(),
            }));
        }
        public IEnumerable<SelectListItem> GetFilterDropDownList()
        {
            IEnumerable<SelectListItem> methods = new List<SelectListItem>
            {
                new() { Value = "", Text = "Por método de pago", Selected = true }
            };
            return methods.Concat(_db.PaymentMethods.OrderBy(x => x.ID).Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Name,
            }));
        }
    }
}