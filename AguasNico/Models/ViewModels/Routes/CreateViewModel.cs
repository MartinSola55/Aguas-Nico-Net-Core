using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Routes
{
    public class CreateViewModel
    {
        public List<ApplicationUser> Dealers = [];
        public Route Route = new();
        public List<SelectListItem> Days { get; set; } = new ConstantsMethods().GetDaysDropdown(valueString: false, firstItem: new() { Text = "Seleccione un día", Value = "", Disabled = true, Selected = true });
    }
}
