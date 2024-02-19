using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Routes
{
    public class EditViewModel
    {
        public Route Route = new();
        public List<Client> ClientsInRoute { get; set; } = [];
        public List<Client> ClientsNotInRoute { get; set; } = [];
    }
}
