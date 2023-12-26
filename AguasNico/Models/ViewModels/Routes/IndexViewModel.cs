using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Routes
{
    public class IndexViewModel
    {
        public ApplicationUser User = null!;
        public IEnumerable<Route> Routes = new List<Route>();
        public IEnumerable<SelectListItem> Days { get; set; } = new List<SelectListItem>
        {
            new() { Text = "Seleccione un día", Value = "", Disabled = true, Selected = true },
            new() { Text = Day.Lunes.ToString(), Value = Day.Lunes.ToString(), Selected = DateTime.UtcNow.AddHours(-3).DayOfWeek == DayOfWeek.Monday ? true : false },
            new() { Text = Day.Martes.ToString(), Value = Day.Martes.ToString(), Selected = DateTime.UtcNow.AddHours(-3).DayOfWeek == DayOfWeek.Tuesday ? true : false },
            new() { Text = Day.Miércoles.ToString(), Value = Day.Miércoles.ToString(), Selected = DateTime.UtcNow.AddHours(-3).DayOfWeek == DayOfWeek.Wednesday ? true : false },
            new() { Text = Day.Jueves.ToString(), Value = Day.Jueves.ToString(), Selected = DateTime.UtcNow.AddHours(-3).DayOfWeek == DayOfWeek.Thursday ? true : false },
            new() { Text = Day.Viernes.ToString(), Value = Day.Viernes.ToString(), Selected = DateTime.UtcNow.AddHours(-3).DayOfWeek == DayOfWeek.Friday ? true : false },
        };
    }
}
