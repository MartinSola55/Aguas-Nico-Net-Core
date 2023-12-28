using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Stats
{
    public class IndexViewModel
    {
        public required JsonResult AnnualProfits { get; set; }
        public required JsonResult MonthlyProfits { get; set; }
        public IEnumerable<SelectListItem> Years { get; set; } = new List<SelectListItem>();
    }
}