using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using AguasNico.Models.ViewModels.Stats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace AguasNico.Controllers
{
    [Authorize]
    public class StatsController(IWorkContainer workContainer, SignInManager<ApplicationUser> signInManager) : Controller
    {
        private readonly IWorkContainer _workContainer = workContainer;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private BadRequestObjectResult CustomBadRequest(string title, string message, string? error = null)
        {
            return BadRequest(new
            {
                success = false,
                title,
                message,
                error,
            });
        }

        [HttpGet]
        [ActionName("Index")]
        public IActionResult Index()
        {
            try
            {
                DateTime today = DateTime.UtcNow.AddHours(-3);
                IndexViewModel viewModel = new()
                {
                    Years = _workContainer.Route.GetYears(),
                    AnnualProfits = this.GetAnnualProfits(today.Year.ToString()),
                    MonthlyProfits = this.GetMonthlyProfits(today.Year.ToString(), today.Month.ToString()),

                };
                return View(viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        [HttpGet]
        [ActionName("Product")]
        public IActionResult Product(long id)
        {
            try
            {
                ProductViewModel viewModel = new()
                {
                    Product = _workContainer.Product.GetFirstOrDefault(p => p.ID == id),
                    ClientStock = _workContainer.Product.GetClientStock(id),
                    TotalSold = _workContainer.Product.GetTotalSold(id, DateTime.UtcNow.AddHours(-3)),
                    Chart = _workContainer.Product.GetAnnualSales(id, DateTime.UtcNow.AddHours(-3)),
                };
                return View("~/Views/Products/Stats.cshtml", viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        #region Pegadas AJAX

        [HttpGet]
        public JsonResult GetAnnualProfits(string yearString)
        {
            IEnumerable<Cart> allCarts = _workContainer.Cart.GetAll(x => !x.IsStatic && x.CreatedAt.Year.ToString() == yearString, includeProperties: "PaymentMethods");

            // Agrupar las ventas por mes y calcular la suma de Amount
            var cartsByMonth = allCarts
                .GroupBy(x => new { x.CreatedAt.Year, x.CreatedAt.Month })
                .Select(group => new
                {
                    Period = $"{group.Key.Year}-{group.Key.Month.ToString().PadLeft(2, '0')}",
                    Profit = group.Sum(x => x.PaymentMethods.Sum(y => y.Amount)),
                })
                .OrderBy(entry => entry.Period)
                .ToList();

            List<object> annualProfits = [];

            for (int month = 1; month <= 12; month++)
            {
                string monthPadded = month.ToString().PadLeft(2, '0');
                string period = $"{yearString}-{monthPadded}";

                var cartsEntry = cartsByMonth.FirstOrDefault(entry => entry.Period == period);

                decimal sold = (cartsEntry?.Profit ?? 0);

                annualProfits.Add(new { period, sold });
            }

            return Json(new
            {
                success = true,
                data = annualProfits,
            });
        }

        [HttpGet]
        public JsonResult GetMonthlyProfits(string yearString, string monthString)
        {
            IEnumerable<Cart> allCarts = _workContainer.Cart.GetAll(x => !x.IsStatic && x.CreatedAt.Year.ToString() == yearString && x.CreatedAt.Month.ToString() == monthString, includeProperties: "PaymentMethods");

            int year = int.Parse(yearString);
            int month = int.Parse(monthString);

            // Agrupar las ventas por día y calcular la suma de Amount
            var cartsByDay = allCarts
                .GroupBy(x => x.CreatedAt.Day)
                .Select(group => new
                {
                    Day = group.Key,
                    Profit = group.Sum(x => x.PaymentMethods.Sum(y => y.Amount)),
                })
                .OrderBy(entry => entry.Day)
                .ToList();

            List<object> monthlyProfits = [];

            for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
            {
                var cart = cartsByDay.FirstOrDefault(s => s.Day == day);

                decimal sold = (cart?.Profit ?? 0);

                string monthPadded = month.ToString().PadLeft(2, '0');
                string dayPadded = day.ToString().PadLeft(2, '0');
                string period = $"{yearString}-{monthPadded}-{dayPadded}";

                var dailySaleObject = new { period, sold };

                monthlyProfits.Add(dailySaleObject);
            }

            return Json(new
            {
                success = true,
                data = monthlyProfits,
            });
        }

        #endregion
    }
}
