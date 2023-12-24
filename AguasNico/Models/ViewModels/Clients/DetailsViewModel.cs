using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Clients
{
    public class DetailsViewModel
    {
        public Client Client { get; set; } = new();
        public IEnumerable<Transfer> Transfers { get; set; } = [];
        public IEnumerable<Cart> Carts { get; set; } = [];
        public IEnumerable<ClientProduct> Products { get; set; } = [];
    }
}
