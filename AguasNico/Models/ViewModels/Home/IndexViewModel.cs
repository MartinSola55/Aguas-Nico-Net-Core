﻿using System.ComponentModel.DataAnnotations;
using AguasNico.Models.ViewModels.Tables;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Home
{
    public class IndexViewModel
    {
        public ApplicationUser User = null!;

        [DisplayFormat(DataFormatString = "${0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal TotalExpenses { get; set; } = 0;

        [DisplayFormat(DataFormatString = "${0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal TotalSold { get; set; } = 0;
        public int CompletedRoutes { get; set; } = 0;   
        public int PendingRoutes { get; set; } = 0;
        public List<SoldProductsTable> SoldProducts { get; set; } = [];
        public List<Expense> Expenses { get; set; } = [];
        public List<Route> DealerRoutes { get; set; } = [];
        public List<SelectListItem> Days { get; set; } = new ConstantsMethods().GetDaysDropdown(valueString: false, selectByDay: true, firstItem: new() { Text = "Seleccione un día", Value = "", Disabled = true, Selected = true });
    }
}
