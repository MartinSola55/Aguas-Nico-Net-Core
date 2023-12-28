using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AguasNico.Models.ViewModels.Stats
{
    public class ProductViewModel
    {
        public required Product Product { get; set; }
        
        [DisplayFormat(DataFormatString = "${0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal TotalSold { get; set; } = 0;
        public int ClientStock = 0;
        public required int[] Chart { get; set; }
    }
}