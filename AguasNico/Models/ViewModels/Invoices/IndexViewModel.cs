using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Invoices
{
    public class IndexViewModel
    {
        public IEnumerable<SelectListItem> Days = [];
        public IEnumerable<SelectListItem> Dealers = [];
    }
}
