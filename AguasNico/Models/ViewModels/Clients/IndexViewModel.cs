using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Clients
{
    public class IndexViewModel
    {
        public IEnumerable<Client> Clients { get; set; } = new List<Client>();
    }
}
