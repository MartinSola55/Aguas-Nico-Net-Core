using AguasNico.Models.ViewModels.Tables;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AguasNico.Models.ViewModels.Routes.Details
{
    public class DealerViewModel
    {
        public Route Route = new();
        public IEnumerable<SelectListItem> CartStates { get; set; } = new List<SelectListItem>
        {
            new() { Text = "Por estado", Value = "", Selected = true },
            new() { Text = State.Pending.GetDisplayName(), Value = State.Pending.ToString() },
            new() { Text = State.Confirmed.GetDisplayName(), Value = State.Confirmed.ToString() },
            new() { Text = State.Ausent.GetDisplayName(), Value = State.Ausent.ToString() },
            new() { Text = State.NotNeeded.GetDisplayName(), Value = State.NotNeeded.ToString() },
            new() { Text = State.NotNeeded.GetDisplayName(), Value = State.NotNeeded.ToString() },
            new() { Text = State.Holidays.GetDisplayName(), Value = State.Holidays.ToString() }
        };
        public IEnumerable<SelectListItem> ProductTypes { get; set; } = new List<SelectListItem>
        {
            new() { Text = "Por producto", Value = "", Selected = true },
            new() { Text = ProductType.B12L.GetDisplayName(), Value = ProductType.B12L.ToString() },
            new() { Text = ProductType.B20L.GetDisplayName(), Value = ProductType.B20L.ToString() },
            new() { Text = ProductType.Máquina.GetDisplayName(), Value = ProductType.Máquina.ToString() },
            new() { Text = ProductType.Soda.GetDisplayName(), Value = ProductType.Soda.ToString() }
        };
    }
}
