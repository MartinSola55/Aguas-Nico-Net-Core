using System.ComponentModel.DataAnnotations;
using AguasNico.Models.ViewModels.Tables;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Home
{
    public class IndexViewModel
    {
        public ApplicationUser User = null!;

        [DisplayFormat(DataFormatString = "${0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal TotalSold { get; set; } = 0;
        public List<SoldProductsTable> SoldProducts { get; set; } = [];
        public List<Expense> Expenses { get; set; } = [];
        public List<Route> DealerRoutes { get; set; } = [];
        public List<SelectListItem> Days { get; set; } = new ConstantsMethods().GetDaysDropdown(valueString: false, selectByDay: true, firstItem: new() { Text = "Seleccione un día", Value = "", Disabled = true, Selected = true });
        public List<SelectListItem> Dealers { get; set; } = [];
    }
}
