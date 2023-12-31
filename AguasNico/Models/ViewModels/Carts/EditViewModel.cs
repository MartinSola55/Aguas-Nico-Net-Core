using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Carts
{
    public class EditViewModel
    {
        public Cart Cart { get; set; } = new();
        public List<CartProduct> Products { get; set; } = [];
        public List<ReturnedProduct> ReturnedProducts { get; set; } = [];
        public IEnumerable<SelectListItem> PaymentMethodsDropDown { get; set; } = [];
    }
}
