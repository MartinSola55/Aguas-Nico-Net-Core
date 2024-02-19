using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Invoices
{
    public class IndexViewModel
    {
        public List<SelectListItem> Days { get; set; } = new ConstantsMethods().GetDaysDropdown(valueString: false, firstItem: new() { Text = "Seleccione un día", Value = "", Disabled = true, Selected = true });
        public List<SelectListItem> Dealers = [];
    }
}
