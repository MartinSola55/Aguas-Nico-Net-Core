using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using AguasNico.Models.ViewModels.Dealers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace AguasNico.Controllers
{
    [Authorize(Roles = Constants.Admin)]
    public class DealersController(IWorkContainer workContainer) : Controller
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
                    Dealers = [.. dealers.OrderByDescending(x => x.UserName) ]
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
                var dealer = await _workContainer.ApplicationUser.GetFirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("No se ha encontrado el usuario");
                DetailsViewModel viewModel = new()
                {
                    Dealer = dealer,
                    TotalCarts = await _workContainer.Dealer.GetTotalCarts(dealer.Id, DateTime.UtcNow.AddHours(-3)),
                    CompletedCarts = await _workContainer.Dealer.GetTotalCompletedCarts(dealer.Id, DateTime.UtcNow.AddHours(-3)),
                    PendingCarts = await _workContainer.Dealer.GetTotalPendingCarts(dealer.Id, DateTime.UtcNow.AddHours(-3)),
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
        public async Task<IActionResult> GetClientsByDay(Day day)
        {
            try
            {
                var clients = await _workContainer.Client.GetAllAsync(x => x.DeliveryDay == day);

                return Json(new
                {
                    success = true,
                    data = clients.Select(x => new
                    {
                        id = x.ID,
                        name = x.Name,
                        debt = x.Debt,
                    })
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

                return Json(new
                {
                    success = true,
                    data = clients.Select(x => new
                    {
                        id = x.ID,
                        name = x.Name,
                        address = x.Address,
                    })
                });
            }
            catch (Exception)
            {
                return CustomBadRequest(title: "No se encontraron los clientes", message: "Intente nuevamente o comuníquese para soporte");
            }
        }

        #endregion
    }
}
