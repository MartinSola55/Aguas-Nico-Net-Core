using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Products
{
    public class IndexViewModel
    {
        public ApplicationUser User = new();
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public Product CreateViewModel { get; set; } = new();
        public List<SelectListItem> ProductTypes { get; set; } = [];
    }
}
