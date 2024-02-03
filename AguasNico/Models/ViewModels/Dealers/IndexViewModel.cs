using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Dealers
{
    public class IndexViewModel
    {
        public List<ApplicationUser> Dealers { get; set; } = [];
    }
}
