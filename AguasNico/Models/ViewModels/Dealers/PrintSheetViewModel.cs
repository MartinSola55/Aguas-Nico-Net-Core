using System.ComponentModel.DataAnnotations;
using AguasNico.Models.ViewModels.Tables;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Dealers
{
    public class PrintSheetViewModel
    {
        public ApplicationUser Dealer { get; set; } = new();
        public List<DealerSheet> MondaySheets { get; set; } = [];
        public List<DealerSheet> TuesdaySheets { get; set; } = [];
        public List<DealerSheet> WednesdaySheets { get; set; } = [];
        public List<DealerSheet> ThursdaySheets { get; set; } = [];
        public List<DealerSheet> FridaySheets { get; set; } = [];
    }
}
