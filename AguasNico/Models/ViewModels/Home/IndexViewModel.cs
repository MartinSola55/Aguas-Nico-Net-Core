using System.ComponentModel.DataAnnotations;
using AguasNico.Models.ViewModels.Tables;

namespace AguasNico.Models.ViewModels.Home
{
    public class IndexViewModel
    {
        public ApplicationUser User = null!;
        
        [DisplayFormat(DataFormatString = "{0:F0}", ApplyFormatInEditMode = true)]
        public decimal TotalExpenses { get; set; } = 0;

        [DisplayFormat(DataFormatString = "{0:F0}", ApplyFormatInEditMode = true)]
        public decimal TotalSold { get; set; } = 0;
        public decimal TotalSpent { get; set; } = 0;
        public int CompletedRoutes { get; set; } = 0;   
        public int PendingRoutes { get; set; } = 0;
        public List<SoldProductsTable> SoldProducts { get; set; } = [];
    }
}
