using AguasNico.Models.ViewModels.Tables;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AguasNico.Models.ViewModels.Routes.Details
{
    public class DealerViewModel
    {
        public Route Route = new();
        public Cart Cart = new();
        public IEnumerable<SelectListItem> CartStates { get; set; } = new List<SelectListItem>
        {
            new() { Text = "Por estado", Value = "", Selected = true },
            new() { Text = State.Pending.GetDisplayName(), Value = State.Pending.GetDisplayName() },
            new() { Text = State.Confirmed.GetDisplayName(), Value = State.Confirmed.GetDisplayName() },
            new() { Text = State.Ausent.GetDisplayName(), Value = State.Ausent.GetDisplayName() },
            new() { Text = State.NotNeeded.GetDisplayName(), Value = State.NotNeeded.GetDisplayName() },
            new() { Text = State.NotNeeded.GetDisplayName(), Value = State.NotNeeded.GetDisplayName() },
            new() { Text = State.Holidays.GetDisplayName(), Value = State.Holidays.GetDisplayName() }
        };
        public IEnumerable<SelectListItem> ProductTypes { get; set; } = new List<SelectListItem>
        {
            new() { Text = "Por producto", Value = "", Selected = true },
            new() { Text = ProductType.B12L.GetDisplayName(), Value = ProductType.B12L.ToString() },
            new() { Text = ProductType.B20L.GetDisplayName(), Value = ProductType.B20L.ToString() },
            new() { Text = ProductType.Máquina.GetDisplayName(), Value = ProductType.Máquina.ToString() },
            new() { Text = ProductType.Soda.GetDisplayName(), Value = ProductType.Soda.ToString() }
        };
        public IEnumerable<SelectListItem> PaymentMethods { get; set; } = [];
        public List<State> States { get; set; } = [];
    }
}
