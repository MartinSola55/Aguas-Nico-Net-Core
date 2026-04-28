using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Invoices
{
    public class IndexViewModel
    {
        public List<SelectListItem> Days { get; set; } = new ConstantsMethods().GetDaysDropdown(valueString: false, firstItem: new() { Text = "Todos los dias", Value = "", Disabled = false, Selected = true });
        public List<SelectListItem> Dealers = [];
    }
}
