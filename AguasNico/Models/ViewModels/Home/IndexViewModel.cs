﻿namespace AguasNico.Models.ViewModels.Home
{
    public class IndexViewModel
    {
        public ApplicationUser User = null!;
        public decimal TotalTransfers { get; set; } = 0;
        public decimal TotalSold { get; set; } = 0;
        public decimal TotalSpent { get; set; } = 0;
        public int CompletedRoutes { get; set; } = 0;   
        public int PendingRoutes { get; set; } = 0;
        public Tuple<Product, int[]> Products { get; set; } = null!;
    }
}