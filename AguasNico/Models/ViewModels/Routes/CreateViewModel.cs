using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Routes
{
    public class CreateViewModel
    {
        public IEnumerable<ApplicationUser> Dealers = [];
        public Route Route = new();
        public IEnumerable<SelectListItem> Days { get; set; } = new List<SelectListItem>
        {
            new() { Text = "Seleccione un día", Value = "", Disabled = true, Selected = true },
            new() { Text = Day.Lunes.ToString(), Value = ((int)Day.Lunes).ToString() },
            new() { Text = Day.Martes.ToString(), Value = ((int)Day.Martes).ToString() },
            new() { Text = Day.Miércoles.ToString(), Value = ((int)Day.Miércoles).ToString() },
            new() { Text = Day.Jueves.ToString(), Value = ((int)Day.Jueves).ToString() },
            new() { Text = Day.Viernes.ToString(), Value = ((int)Day.Viernes).ToString() },
        };
    }
}
