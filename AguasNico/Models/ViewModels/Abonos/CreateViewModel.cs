using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Abonos
{
    public class CreateViewModel
    {
        public Abono Abono { get; set; } = new();
        public List<ProductType> Products { get; set; } = new ConstantsMethods().GetProductTypes();
    }
}
