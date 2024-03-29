﻿using AguasNico.Models.ViewModels.Tables;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AguasNico.Models.ViewModels.Routes.Details
{
    public class DealerViewModel
    {
        public Route Route = new();
        public List<ClientData> Clients = [];
        public Cart Cart = new();
        public List<SelectListItem> PaymentTypes { get; set; } = [];
        public List<SelectListItem> CartStates { get; set; } = new ConstantsMethods().GetStatesDropdown(valueString: true, firstItem: new() { Text = "Por estado", Value = "", Selected = true });
        public List<SelectListItem> ProductTypes { get; set; } = new ConstantsMethods().GetProductTypesDropdown(valueString: true, firstItem: new() { Text = "Por producto", Value = "", Selected = true });
        public List<SelectListItem> PaymentMethods { get; set; } = [];
        public List<State> States { get; set; } = [];
    }
}
