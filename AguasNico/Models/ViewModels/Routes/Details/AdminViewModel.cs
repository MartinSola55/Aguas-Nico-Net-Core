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
        public List<SelectListItem> PaymentTypes { get; set; } = [];
        public List<Transfer> Transfers { get; set; } = [];

        public int CompletedCarts { get; set; } = 0;
        public int PendingCarts { get; set; } = 0;
        public List<SelectListItem> CartStates { get; set; } = new ConstantsMethods().GetStatesDropdown(valueString: true, firstItem: new () { Text = "Por estado", Value = "", Selected = true });
        public List<SelectListItem> ProductTypes { get; set; } = new ConstantsMethods().GetProductTypesDropdown(valueString: true, firstItem: new() { Text = "Por producto", Value = "", Selected = true });
    }
}
