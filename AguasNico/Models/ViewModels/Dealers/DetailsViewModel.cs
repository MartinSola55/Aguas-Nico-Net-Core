using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Dealers
{
    public class DetailsViewModel
    {
        public ApplicationUser Dealer { get; set; } = new();
        public int TotalCarts { get; set; } = 0;
        public int CompletedCarts { get; set; } = 0;
        public int PendingCarts { get; set; } = 0;
        [DisplayFormat(DataFormatString = "${0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal TotalCollected { get; set; } = 0;
        public IEnumerable<SelectListItem> Days { get; set; } = new List<SelectListItem>
        {
            new() { Text = "Seleccione un día", Value = "", Disabled = true, Selected = true },
            new() { Text = Day.Lunes.ToString(), Value = Day.Lunes.ToString() },
            new() { Text = Day.Martes.ToString(), Value = Day.Martes.ToString() },
            new() { Text = Day.Miércoles.ToString(), Value = Day.Miércoles.ToString() },
            new() { Text = Day.Jueves.ToString(), Value = Day.Jueves.ToString() },
            new() { Text = Day.Viernes.ToString(), Value = Day.Viernes.ToString() },
        };
    }
}
