using AguasNico.Models.ViewModels.Home;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AguasNico.Controllers
{
    [Authorize]
    public class HomeController(IWorkContainer workContainer, SignInManager<ApplicationUser> signInManager, IConfiguration configuration) : BaseController(configuration)
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
                        var expenses = await _workContainer.Expense.GetAllAsync(x => x.CreatedAt.Date == DateTime.UtcNow.AddHours(-3).Date, includeProperties: "User");
                        viewModel.Dealers = await _workContainer.ApplicationUser.GetDealersDropDownList();
                        viewModel.SoldProducts = await _workContainer.Tables.GetSoldProductsByDate(DateTime.UtcNow.AddHours(-3).Date);
                        viewModel.Expenses = expenses.OrderByDescending(x => x.Amount).ToList();
                        viewModel.Payments = await _workContainer.Route.GetTotalCollected(DateTime.UtcNow.AddHours(-3).Date);
                        viewModel.Transfers = await _workContainer.Transfer.GetAllAsync(x => x.Date.Date == DateTime.UtcNow.AddHours(-3).Date, includeProperties: "Client");
                        viewModel.TotalSold = viewModel.Payments.Sum(x => x.Amount) + viewModel.Transfers.Sum(x => x.Amount);
                        return View("~/Views/Home/Admin/Index.cshtml", viewModel);
                    case Constants.Dealer:
                        var dealerRoutes = await _workContainer.Route.GetAllAsync(x => x.UserID == user.Id && !x.IsStatic && x.DayOfWeek == today, includeProperties: "User, Carts, Carts.PaymentMethods");
                        viewModel.DealerRoutes = [.. dealerRoutes.OrderByDescending(x => x.CreatedAt)];
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
