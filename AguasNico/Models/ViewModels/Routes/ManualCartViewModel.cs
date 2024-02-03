using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Routes
{
    public class ManualCartViewModel
    {
        public Route Route = new();
        public List<Client> Clients = [];
        public List<SelectListItem> PaymentMethods { get; set; } = [];
    }
}
