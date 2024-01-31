using AguasNico.Models.ViewModels.Home;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AguasNico.Controllers
{
    [Authorize]
    public class HomeController(IWorkContainer workContainer, SignInManager<ApplicationUser> signInManager) : Controller
    {
        private readonly IWorkContainer _workContainer = workContainer;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

        [HttpGet]
        [Display(Name = "Index")]
        public IActionResult Index()
        {
            try
            {
                ApplicationUser user = _workContainer.ApplicationUser.GetFirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
                string role = _signInManager.UserManager.GetRolesAsync(user).Result.First();
                Day today = (Day)(int)DateTime.UtcNow.AddHours(-3).DayOfWeek;
                IndexViewModel viewModel = new()
                {
                    User = user
                };
                switch (role)
                {
                    case Constants.Admin:
                        viewModel.TotalExpenses = _workContainer.Expense.GetTotalExpenses(DateTime.UtcNow.AddHours(-3).Date);
                        viewModel.TotalSold = _workContainer.Route.GetTotalSold(DateTime.UtcNow.AddHours(-3).Date);
                        viewModel.CompletedRoutes = _workContainer.Route.GetAll(x => x.CreatedAt.Date == DateTime.UtcNow.AddHours(-3).Date && x.Carts.All(x => x.State != State.Pending)).Count();
                        viewModel.PendingRoutes = _workContainer.Route.GetAll(x => x.CreatedAt.Date == DateTime.UtcNow.AddHours(-3).Date && x.Carts.Any(x => x.State == State.Pending)).Count();
                        viewModel.SoldProducts = _workContainer.Tables.GetSoldProductsByDate(DateTime.UtcNow.AddHours(-3).Date);
                        viewModel.Expenses = _workContainer.Expense.GetAll(x => x.CreatedAt.Date == DateTime.UtcNow.AddHours(-3).Date, includeProperties: "User");
                        return View("~/Views/Home/Admin/Index.cshtml", viewModel);
                    case Constants.Dealer:
                        viewModel.DealerRoutes = _workContainer.Route.GetAll(x => x.UserID == user.Id && !x.IsStatic && x.DayOfWeek == today, includeProperties: "User, Carts, Carts.PaymentMethods").OrderByDescending(x => x.CreatedAt);
                        return View("~/Views/Home/Dealer/Index.cshtml", viewModel);
                    default:
                        return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
                }
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }
    }
}
