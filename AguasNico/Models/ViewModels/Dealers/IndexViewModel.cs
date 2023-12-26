using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Dealers
{
    public class IndexViewModel
    {
        public IEnumerable<ApplicationUser> Dealers { get; set; } = new List<ApplicationUser>();
    }
}
