using System.ComponentModel.DataAnnotations;

namespace AguasNico.Models.ViewModels.Dealers
{
    public class DetailsViewModel
    {
        public ApplicationUser Dealer { get; set; } = new();
        public int TotalCarts { get; set; } = 0;
        public int CompletedCarts { get; set; } = 0;
        public int PendingCarts { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:F0}", ApplyFormatInEditMode = true)]
        public decimal TotalCollected { get; set; } = 0;
    }
}
