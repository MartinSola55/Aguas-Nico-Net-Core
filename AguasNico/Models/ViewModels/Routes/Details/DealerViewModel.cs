using AguasNico.Models.ViewModels.Tables;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AguasNico.Models.ViewModels.Routes.Details
{
    public class DealerViewModel
    {
        public Route Route = new();
        public Cart Cart = new();
        public IEnumerable<SelectListItem> PaymentTypes { get; set; } = [];
        public IEnumerable<SelectListItem> CartStates { get; set; } = [];
        public IEnumerable<SelectListItem> ProductTypes { get; set; } = [];
        public IEnumerable<SelectListItem> PaymentMethods { get; set; } = [];
        public List<State> States { get; set; } = [];
    }
}
