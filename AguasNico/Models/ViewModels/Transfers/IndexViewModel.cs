using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Transfers
{
    public class IndexViewModel
    {
        public List<Transfer> Transfers { get; set; } = [];
    }
}
