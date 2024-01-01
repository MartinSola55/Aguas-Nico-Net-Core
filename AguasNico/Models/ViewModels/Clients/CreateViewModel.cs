using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Clients
{
    public class CreateViewModel
    {
        public string Role { get; set; } = null!;
        public Client Client { get; set; } = new();
        public IEnumerable<SelectListItem> Days { get; set; } = new List<SelectListItem> 
        {
            new() { Text = "Seleccione un día", Value = "", Disabled = true, Selected = true },
            new() { Text = Day.Lunes.ToString(), Value = Day.Lunes.ToString() },
            new() { Text = Day.Martes.ToString(), Value = Day.Martes.ToString() },
            new() { Text = Day.Miércoles.ToString(), Value = Day.Miércoles.ToString() },
            new() { Text = Day.Jueves.ToString(), Value = Day.Jueves.ToString() },
            new() { Text = Day.Viernes.ToString(), Value = Day.Viernes.ToString() },
        };
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public IEnumerable<SelectListItem> Dealers { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> InvoiceTyes { get; set; } = new List<SelectListItem>
        {
            new() { Text = "Seleccione un tipo de factura", Value = "", Disabled = true, Selected = true },
            new() { Text = InvoiceType.A.ToString(), Value = InvoiceType.A.ToString() },
            new() { Text = InvoiceType.B.ToString(), Value = InvoiceType.B.ToString() },
        };
        public IEnumerable<SelectListItem> TaxConditions { get; set; } = new List<SelectListItem>
        {
            new() { Text = "Seleccione una condición de IVA", Value = "", Disabled = true, Selected = true },
            new() { Text = TaxCondition.RI.GetDisplayName(), Value = TaxCondition.RI.ToString() },
            new() { Text = TaxCondition.MO.GetDisplayName(), Value = TaxCondition.MO.ToString() },
            new() { Text = TaxCondition.EX.GetDisplayName(), Value = TaxCondition.EX.ToString() },
            new() { Text = TaxCondition.CF.GetDisplayName(), Value = TaxCondition.CF.ToString() },
        };
    }
}
