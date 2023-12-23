using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Routes
{
    public class DetailsViewModel
    {
        public IEnumerable<ApplicationUser> Dealers = [];
        public IEnumerable<SelectListItem> PaymentMethods = [];
        public IEnumerable<DispatchedProduct> DispatchedProducts = [];
        public Route Route = new();
    }
}
