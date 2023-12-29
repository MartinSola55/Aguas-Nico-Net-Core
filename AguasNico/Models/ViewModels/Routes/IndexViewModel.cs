using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Routes
{
    public class IndexViewModel
    {
        public ApplicationUser User = null!;
        public IEnumerable<Route> Routes = new List<Route>();
        public IEnumerable<SelectListItem> DaysDropDown { get; set; } = new List<SelectListItem>
        {
            new() { Text = "Seleccione un día", Value = "", Disabled = true, Selected = true },
            new() { Text = Day.Lunes.ToString(), Value = Day.Lunes.ToString(), Selected = DateTime.UtcNow.AddHours(-3).DayOfWeek == DayOfWeek.Monday },
            new() { Text = Day.Martes.ToString(), Value = Day.Martes.ToString(), Selected = DateTime.UtcNow.AddHours(-3).DayOfWeek == DayOfWeek.Tuesday },
            new() { Text = Day.Miércoles.ToString(), Value = Day.Miércoles.ToString(), Selected = DateTime.UtcNow.AddHours(-3).DayOfWeek == DayOfWeek.Wednesday },
            new() { Text = Day.Jueves.ToString(), Value = Day.Jueves.ToString(), Selected = DateTime.UtcNow.AddHours(-3).DayOfWeek == DayOfWeek.Thursday },
            new() { Text = Day.Viernes.ToString(), Value = Day.Viernes.ToString(), Selected = DateTime.UtcNow.AddHours(-3).DayOfWeek == DayOfWeek.Friday },
        };
        public List<Day> Days =
        [
            Day.Lunes,
            Day.Martes,
            Day.Miércoles,
            Day.Jueves,
            Day.Viernes
        ];
    }
}
