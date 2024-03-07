using System.ComponentModel.DataAnnotations;
using AguasNico.Models.ViewModels.Tables;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Dealers
{
    public class PrintSheetViewModel
    {
        public ApplicationUser Dealer { get; set; } = new();
        public List<DealerSheet> Sheets { get; set; } = [];
        public List<SelectListItem> Days { get; set; } = new ConstantsMethods().GetDaysDropdown(valueString: true, selectByDay: false, firstItem: new() { Text = "Todas las planillas", Value = "", Disabled = false, Selected = true });
    }
}
