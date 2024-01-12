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
        public IEnumerable<SelectListItem> Days { get; set; } = new List<SelectListItem>
        {
            new() { Text = "Seleccione un día", Value = "", Disabled = true, Selected = true },
            new() { Text = Day.Lunes.ToString(), Value = Day.Lunes.ToString() },
            new() { Text = Day.Martes.ToString(), Value = Day.Martes.ToString() },
            new() { Text = Day.Miércoles.ToString(), Value = Day.Miércoles.ToString() },
            new() { Text = Day.Jueves.ToString(), Value = Day.Jueves.ToString() },
            new() { Text = Day.Viernes.ToString(), Value = Day.Viernes.ToString() },
        };
        // Objeto para el historial de botellas
        public IEnumerable<ProductHistory> ProductsHistory { get; set; } = [];        
    }
}
