using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Clients
{
    public class IndexViewModel
    {
        public List<Client> Clients { get; set; } = [];
    }
}
