﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Carts
{
    public class EditViewModel
    {
        public Cart Cart { get; set; } = new();
        public List<CartProduct> Products { get; set; } = [];
        public List<CartAbonoProduct> AbonoProducts { get; set; } = [];
        public List<ReturnedProduct> ReturnedProducts { get; set; } = [];
        public List<SelectListItem> PaymentMethodsDropDown { get; set; } = [];
    }
}
