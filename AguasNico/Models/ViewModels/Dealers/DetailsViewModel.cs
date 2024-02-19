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
        public List<SelectListItem> Days { get; set; } = new ConstantsMethods().GetDaysDropdown(valueString: true, firstItem: new() { Text = "Seleccione un día", Value = "", Disabled = true, Selected = true });
    }
}
