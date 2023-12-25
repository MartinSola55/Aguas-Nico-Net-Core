using AguasNico.Models.ViewModels.Home;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AguasNico.Controllers
{
    [Authorize]
    public class HomeController(IWorkContainer workContainer, SignInManager<ApplicationUser> signInManager) : Controller
    {
        private readonly IWorkContainer _workContainer = workContainer;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                ApplicationUser user = _workContainer.ApplicationUser.GetFirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
                string role = _signInManager.UserManager.GetRolesAsync(user).Result.First();
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
                        break;
                    case Constants.Dealer:
                        
                        break;
                    default:
                        return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
                }
                
                return View(viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }
    }
}
