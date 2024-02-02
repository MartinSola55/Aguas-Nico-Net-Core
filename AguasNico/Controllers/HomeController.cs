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
        public async Task<IActionResult> Index()
        {
            try
            {
                var sessionUser = User.Identity ?? throw new Exception("No se pudo obtener el usuario de la sesión");
                var user = await _workContainer.ApplicationUser.GetFirstOrDefaultAsync(x => x.UserName != null && x.UserName.Equals(sessionUser.Name));

                var role = _signInManager.UserManager.GetRolesAsync(user).Result.First();
                var today = (Day)(int)DateTime.UtcNow.AddHours(-3).DayOfWeek;

                IndexViewModel viewModel = new()
                {
                    User = user
                };
                switch (role)
                {
                    case Constants.Admin:
                        var completedRoutes = await _workContainer.Route.GetAllAsync(x => x.CreatedAt.Date == DateTime.UtcNow.AddHours(-3).Date && x.Carts.All(x => x.State != State.Pending));
                        var pendingRoutes = await _workContainer.Route.GetAllAsync(x => x.CreatedAt.Date == DateTime.UtcNow.AddHours(-3).Date && x.Carts.Any(x => x.State == State.Pending));

                        viewModel.TotalExpenses = await _workContainer.Expense.GetTotalExpenses(DateTime.UtcNow.AddHours(-3).Date);
                        viewModel.TotalSold = await _workContainer.Route.GetTotalSold(DateTime.UtcNow.AddHours(-3).Date);
                        viewModel.CompletedRoutes = completedRoutes.Count();
                        viewModel.PendingRoutes = pendingRoutes.Count();
                        viewModel.SoldProducts = await _workContainer.Tables.GetSoldProductsByDate(DateTime.UtcNow.AddHours(-3).Date);
                        viewModel.Expenses = await _workContainer.Expense.GetAllAsync(x => x.CreatedAt.Date == DateTime.UtcNow.AddHours(-3).Date, includeProperties: "User");
                        return View("~/Views/Home/Admin/Index.cshtml", viewModel);
                    case Constants.Dealer:
                        var dealerRoutes = await _workContainer.Route.GetAllAsync(x => x.UserID == user.Id && !x.IsStatic && x.DayOfWeek == today, includeProperties: "User, Carts, Carts.PaymentMethods");
                        viewModel.DealerRoutes = dealerRoutes.OrderByDescending(x => x.CreatedAt);
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
