using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Abonos
{
    public class IndexViewModel
    {
        public ApplicationUser User = new();
        public List<Abono> Abonos { get; set; } = [];
        public Abono EditedAbono { get; set; } = new();
    }
}
