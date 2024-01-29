using System.ComponentModel.DataAnnotations;
using AguasNico.Models.ViewModels.Tables;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Clients
{
    public enum ActionType
    {
        [Display(Name = "Baja")] Baja,
        [Display(Name = "Devuelve")] Devuelve,
        [Display(Name = "Baja (abono)")] Abono,
    }
    public class ProductHistory
    {
        public ProductType ProductType { get; set; }
        public ActionType ActionType { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
    }
    public class DetailsViewModel
    {
        public Client Client { get; set; } = new();
        public List<CartsTransfersHistoryTable> CartsTransfersHistory { get; set; } = [];
        public IEnumerable<ClientProduct> Products { get; set; } = new List<ClientProduct>();
        public IEnumerable<ClientAbono> Abonos { get; set; } = new List<ClientAbono>();
        public IEnumerable<SelectListItem> Dealers { get; set; } = [];
        public IEnumerable<SelectListItem> InvoiceTypes { get; set; } = new ConstantsMethods().GetInvoiceTypesDropdown(valueString: false, firstItem: new() { Text = "Seleccione un tipo de factura", Value = "", Disabled = true, Selected = true });
        public IEnumerable<SelectListItem> TaxConditions { get; set; } = new ConstantsMethods().GetTaxConditionsDropdown(valueString: false, firstItem: new() { Text = "Seleccione una condición de IVA", Value = "", Disabled = true, Selected = true });
        public IEnumerable<SelectListItem> Days { get; set; } = new ConstantsMethods().GetDaysDropdown(valueString: false, firstItem: new() { Text = "Seleccione un día", Value = "", Disabled = true, Selected = true });
        // Objeto para el historial de botellas
        public IEnumerable<ProductHistory> ProductsHistory { get; set; } = [];        
    }
}
