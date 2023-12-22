using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Expenses
{
    public class IndexViewModel
    {
        public ApplicationUser User = new();
        public IEnumerable<Expense> Expenses { get; set; } = new List<Expense>();
        public IEnumerable<SelectListItem> Dealers { get; set; } = new List<SelectListItem>();
        public Expense CreateViewModel { get; set; } = new();
    }
}
