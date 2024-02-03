using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Routes
{
    public class IndexViewModel
    {
        public ApplicationUser User = null!;
        public List<Route> Routes = [];
        public List<SelectListItem> DaysDropDown { get; set; } = new ConstantsMethods().GetDaysDropdown(valueString: false, selectByDay: true, firstItem: new() { Text = "Seleccione un día", Value = "", Disabled = true, Selected = true });
        public List<Day> Days = new ConstantsMethods().GetDays();
    }
}
