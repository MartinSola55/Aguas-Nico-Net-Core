using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Clients
{
    public class CreateViewModel
    {
        public string Role { get; set; } = null!;
        public Client Client { get; set; } = new();
        public IEnumerable<SelectListItem> Days { get; set; } = new ConstantsMethods().GetDaysDropdown(valueString: false, firstItem: new() { Text = "Seleccione un día", Value = "", Disabled = true, Selected = true });
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public IEnumerable<SelectListItem> Dealers { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> InvoiceTypes { get; set; } = new ConstantsMethods().GetInvoiceTypesDropdown(valueString: false, firstItem: new() { Text = "Seleccione un tipo de factura", Value = "", Disabled = true, Selected = true });
        public IEnumerable<SelectListItem> TaxConditions { get; set; } = new ConstantsMethods().GetTaxConditionsDropdown(valueString: false, firstItem: new() { Text = "Seleccione una condición de IVA", Value = "", Disabled = true, Selected = true });
    }
}
