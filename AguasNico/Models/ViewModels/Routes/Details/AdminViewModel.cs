using AguasNico.Models.ViewModels.Tables;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace AguasNico.Models.ViewModels.Routes.Details
{
    public class AdminViewModel
    {
        public Route Route = new();
        public List<SoldProductsTable> SoldProducts { get; set; } = [];

        [DisplayFormat(DataFormatString = "${0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal TotalSold { get; set; } = 0;

        [DisplayFormat(DataFormatString = "${0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal TotalExpenses { get; set; } = 0;

        public List<CartPaymentMethod> Payments { get; set; } = [];
        public IEnumerable<SelectListItem> PaymentTypes { get; set; } = [];
        public IEnumerable<Transfer> Transfers { get; set; } = [];

        public int CompletedCarts { get; set; } = 0;
        public int PendingCarts { get; set; } = 0;
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
            new() { Text = ProductType.B12L.GetDisplayName(), Value = ProductType.B12L.GetDisplayName()}, //ProductType.B12L.ToString()
            new() { Text = ProductType.B20L.GetDisplayName(), Value = ProductType.B20L.GetDisplayName() },//ProductType.B20L.ToString()
            new() { Text = ProductType.Máquina.GetDisplayName(), Value = ProductType.Máquina.GetDisplayName() },//ProductType.Máquina.ToString()
            new() { Text = ProductType.Soda.GetDisplayName(), Value = ProductType.Soda.GetDisplayName() }//ProductType.Soda.ToString()
        };
    }
}
