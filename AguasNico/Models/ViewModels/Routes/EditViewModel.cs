using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Routes
{
    public class EditViewModel
    {
        public Route Route = new();
        public IEnumerable<Client> ClientsInRoute { get; set; } = new List<Client>();
        public IEnumerable<Client> ClientsNotInRoute { get; set; } = new List<Client>();
    }
}
