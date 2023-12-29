using System.ComponentModel.DataAnnotations;
using AguasNico.Models.ViewModels.Tables;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Home
{
    public class IndexViewModel
    {
        public ApplicationUser User = null!;

        [DisplayFormat(DataFormatString = "${0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal TotalExpenses { get; set; } = 0;

        [DisplayFormat(DataFormatString = "${0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal TotalSold { get; set; } = 0;
        public int CompletedRoutes { get; set; } = 0;   
        public int PendingRoutes { get; set; } = 0;
        public List<SoldProductsTable> SoldProducts { get; set; } = [];
        public IEnumerable<Expense> Expenses { get; set; } = [];
        public IEnumerable<Route> DealerRoutes { get; set; } = [];
        public IEnumerable<SelectListItem> Days { get; set; } = new List<SelectListItem>
        {
            new() { Text = "Seleccione un día", Value = "", Disabled = true, Selected = true },
            new() { Text = Day.Lunes.ToString(), Value = ((int)Day.Lunes).ToString(), Selected = DateTime.UtcNow.AddHours(-3).DayOfWeek == DayOfWeek.Monday },
            new() { Text = Day.Martes.ToString(), Value = ((int)Day.Martes).ToString(), Selected = DateTime.UtcNow.AddHours(-3).DayOfWeek == DayOfWeek.Tuesday },
            new() { Text = Day.Miércoles.ToString(), Value = ((int)Day.Miércoles).ToString(), Selected = DateTime.UtcNow.AddHours(-3).DayOfWeek == DayOfWeek.Wednesday },
            new() { Text = Day.Jueves.ToString(), Value = ((int)Day.Jueves).ToString(), Selected = DateTime.UtcNow.AddHours(-3).DayOfWeek == DayOfWeek.Thursday },
            new() { Text = Day.Viernes.ToString(), Value = ((int)Day.Viernes).ToString(), Selected = DateTime.UtcNow.AddHours(-3).DayOfWeek == DayOfWeek.Friday },
        };
    }
}
