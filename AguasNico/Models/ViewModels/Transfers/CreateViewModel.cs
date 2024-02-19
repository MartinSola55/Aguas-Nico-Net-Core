using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Transfers
{
    public class CreateViewModel
    {
        public Transfer Transfer { get; set; } = new();
        public List<SelectListItem> Dealers { get; set; } = [];
    }
}
