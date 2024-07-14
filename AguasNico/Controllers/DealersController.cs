using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using AguasNico.Models.ViewModels.Dealers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AguasNico.Controllers
{
    [Authorize(Roles = Constants.Admin)]
    public class DealersController(IWorkContainer workContainer, IConfiguration configuration) : BaseController(configuration)
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
                var dealers = await _workContainer.ApplicationUser.GetDealers();
                IndexViewModel viewModel = new()
                {
                    Dealers = [.. dealers.OrderBy(x => x.TruckNumber)]
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var dealer = await _workContainer.ApplicationUser.GetFirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("No se ha encontrado el repartidor");
                DetailsViewModel viewModel = new()
                {
                    Dealer = dealer,
                    TotalCarts = await _workContainer.Dealer.GetTotalCarts(dealer.Id, DateTime.UtcNow.AddHours(-3)),
                    CompletedCarts = await _workContainer.Dealer.GetTotalCompletedCarts(dealer.Id, DateTime.UtcNow.AddHours(-3)),
                    PendingCarts = await _workContainer.Dealer.GetTotalPendingCarts(dealer.Id, DateTime.UtcNow.AddHours(-3)),
                    TotalCollected = await _workContainer.Dealer.GetTotalCollected(dealer.Id, DateTime.UtcNow.AddHours(-3)),
                    TotalDebt = await _workContainer.Dealer.GetClientsDebt(dealer.Id),
                    ClientsStock = await _workContainer.Dealer.GetClientsStock(dealer.Id),
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> PrintSheet(string id)
        {
            try
            {
                var dealer = await _workContainer.ApplicationUser.GetFirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("No se ha encontrado el repartidor");
                PrintSheetViewModel viewModel = new()
                {
                    Dealer = dealer,
                    Sheets = await _workContainer.Dealer.GetDealerSheets(dealer.Id),
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        #endregion

        #region AJAX
        [HttpGet]
        public async Task<IActionResult> GetClientsByDay(Day day, string dealerID)
        {
            try
            {
                var clients = await _workContainer.Client.GetAllAsync(x => x.DeliveryDay == day && x.DealerID == dealerID && x.IsActive);

                return Json(new
                {
                    success = true,
                    data = clients.Select(x => new
                    {
                        id = x.ID,
                        name = x.Name,
                        debt = x.Debt,
                    })
                    .OrderBy(x => x.name)
                });
            }
            catch (Exception)
            {
                return CustomBadRequest(title: "No se encontraron los clientes", message: "Intente nuevamente o comuníquese para soporte");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetClientsNotVisited(string dateFromString, string dateToString, string dealerID)
        {
            try
            {
                var dealer = await _workContainer.ApplicationUser.GetFirstOrDefaultAsync(x => x.Id == dealerID) ?? throw new Exception("No se ha encontrado el repartidor");

                var dateFrom = DateTime.Parse(dateFromString);
                var dateTo = DateTime.Parse(dateToString);

                var clients = await _workContainer.Client.GetNotVisited(dateFrom, dateTo, dealer.Id);
                var totalClients = await _workContainer.Client.GetTotalClients(dealer.Id);
                var totalNotVisited = clients.Count;

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        totalClients,
                        totalNotVisited,
                        clients = clients.Select(x => new
                        {
                            id = x.ID,
                            name = x.Name,
                            address = x.Address,
                        })
                        .OrderBy(x => x.name)
                    }
                });
            }
            catch (Exception)
            {
                return CustomBadRequest(title: "No se encontraron los clientes", message: "Intente nuevamente o comuníquese para soporte");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSoldProducts(string dateFromString, string dateToString, string dealerID)
        {
            try
            {
                var dealer = await _workContainer.ApplicationUser.GetFirstOrDefaultAsync(x => x.Id == dealerID) ?? throw new Exception("No se ha encontrado el repartidor");

                var dateFrom = DateTime.Parse(dateFromString);
                var dateTo = DateTime.Parse(dateToString);

                var products = await _workContainer.Tables.GetSoldProductsBetweenDates(dateFrom, dateTo, dealer.Id);

                return Json(products);
            }
            catch (Exception)
            {
                return CustomBadRequest(title: "No se encontraron los clientes", message: "Intente nuevamente o comuníquese para soporte");
            }
        }

        #endregion
    }
}
