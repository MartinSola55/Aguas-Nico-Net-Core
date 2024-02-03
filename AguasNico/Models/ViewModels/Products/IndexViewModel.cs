using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Products
{
    public class IndexViewModel
    {
        public ApplicationUser User = new();
        public List<Product> Products { get; set; } = [];
        public Product Product { get; set; } = new();
        public List<SelectListItem> ProductTypes { get; set; } = new ConstantsMethods().GetProductTypesDropdown(valueString: false, firstItem: new() { Text = "Seleccione un tipo de producto", Value = "", Disabled = true, Selected = true });
    }
}
