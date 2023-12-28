using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Transfers
{
    public class IndexViewModel
    {
        public IEnumerable<Transfer> Transfers { get; set; } = new List<Transfer>();
        public IEnumerable<SelectListItem> Dealers { get; set; } = new List<SelectListItem>();
        public Transfer CreateViewModel { get; set; } = new();
    }
}
