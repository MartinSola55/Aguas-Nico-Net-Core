using Microsoft.AspNetCore.Mvc.Rendering;

namespace AguasNico.Models.ViewModels.Expenses
{
    public class IndexViewModel
    {
        public ApplicationUser User = new();
        public List<Expense> Expenses { get; set; } = [];
        public List<SelectListItem> Dealers { get; set; } = [];
        public Expense CreateViewModel { get; set; } = new();
    }
}
