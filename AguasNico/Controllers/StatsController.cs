using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using AguasNico.Models.ViewModels.Stats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AguasNico.Controllers
{
    [Authorize(Roles = Constants.Admin)]
    public class StatsController(IWorkContainer workContainer, IConfiguration configuration) : BaseController(configuration)
    {
        private readonly IWorkContainer _workContainer = workContainer;
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

        #region Views

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var today = DateTime.UtcNow.AddHours(-3);
                IndexViewModel viewModel = new()
                {
                    Years = await _workContainer.Route.GetYears(),
                    AnnualProfits = await this.GetAnnualProfits(today.Year.ToString()),
                    MonthlyProfits = await this.GetMonthlyProfits(today.Year.ToString(), today.Month.ToString()),

                };
                return View(viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Product(long id)
        {
            try
            {
                var product = await _workContainer.Product.GetFirstOrDefaultAsync(p => p.ID == id);
                ProductViewModel viewModel = new()
                {
                    Product = product,
                    ClientStock = await _workContainer.Product.GetClientStock(id),
                    TotalSold = await _workContainer.Product.GetTotalSold(product.Type, DateTime.UtcNow.AddHours(-3)),
                    Chart = await _workContainer.Product.GetAnnualSales(product.Type, DateTime.UtcNow.AddHours(-3)),
                };
                return View("~/Views/Products/Stats.cshtml", viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        #endregion

        #region AJAX

        [HttpGet]
        public async Task<JsonResult> GetAnnualProfits(string yearString)
        {
            try
            {
                var allCarts = await _workContainer.Cart.GetAllAsync(x => !x.IsStatic && x.CreatedAt.Year.ToString() == yearString, includeProperties: "PaymentMethods");

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

                var annualProfits = new List<object>();

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
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    title = "Ha ocurrido un error al obtener las estadísticas",
                    message = "Intente nuevamente o comuníquese para soporte",
                });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetMonthlyProfits(string yearString, string monthString)
        {
            try
            {
                var allCarts = await _workContainer.Cart.GetAllAsync(x => !x.IsStatic && x.CreatedAt.Year.ToString() == yearString && x.CreatedAt.Month.ToString() == monthString, includeProperties: "PaymentMethods");

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

                var monthlyProfits = new List<object>();

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
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    title = "Ha ocurrido un error al obtener las estadísticas",
                    message = "Intente nuevamente o comuníquese para soporte",
                });
            }
        }

        #endregion
    }
}
