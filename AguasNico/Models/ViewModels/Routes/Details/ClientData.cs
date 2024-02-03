using AguasNico.Models.ViewModels.Tables;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace AguasNico.Models.ViewModels.Routes.Details
{
    public class ClientData
    {
        public long ClientID { get; set; }
        public List<Product> Products { get; set; } = [];
        public List<Product> AbonoProducts { get; set; } = [];
        public class Product
        {
            public ProductType Type { get; set; }
            public string Name { get; set; } = "";

            [DisplayFormat(DataFormatString = "${0:#,##0}", ApplyFormatInEditMode = true)]
            public decimal Price { get; set; }
            public int Available { get; set; }
        }
    }
}
