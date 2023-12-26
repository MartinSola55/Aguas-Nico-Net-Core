using AguasNico.Models.ViewModels.Tables;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AguasNico.Models.ViewModels.Routes.Details
{
    public class AdminViewModel
    {
        public Route Route = new();
        public List<SoldProductsTable> SoldProducts { get; set; } = [];

        [DisplayFormat(DataFormatString = "{0:F0}", ApplyFormatInEditMode = true)]
        public decimal TotalSold { get; set; } = 0;

        [DisplayFormat(DataFormatString = "{0:F0}", ApplyFormatInEditMode = true)]
        public decimal TotalExpenses { get; set; } = 0;

        public List<CartPaymentMethod> Payments { get; set; } = [];

        public int CompletedCarts { get; set; } = 0;
        public int PendingCarts { get; set; } = 0;
    }
}
