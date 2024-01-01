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

        [HttpGet]
        [ActionName("Index")]
        public IActionResult Index()
        {
            try
            {
                IndexViewModel viewModel = new()
                {
                    Dealers = _workContainer.ApplicationUser.GetDealers().OrderByDescending(x => x.UserName)
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        [HttpGet]
        [ActionName("Details")]
        public IActionResult Details(string id)
        {
            try
            {
                ApplicationUser dealer = _workContainer.ApplicationUser.GetFirstOrDefault(x => x.Id == id) ?? throw new Exception("No se ha encontrado el usuario");
                DetailsViewModel viewModel = new()
                {
                    Dealer = dealer,
                    TotalCarts = _workContainer.Dealer.GetTotalCarts(dealer.Id, DateTime.UtcNow.AddHours(-3)),
                    CompletedCarts = _workContainer.Dealer.GetTotalCompletedCarts(dealer.Id, DateTime.UtcNow.AddHours(-3)),
                    PendingCarts = _workContainer.Dealer.GetTotalPendingCarts(dealer.Id, DateTime.UtcNow.AddHours(-3)),
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        #region Pegadas AJAX
        [HttpGet]
        [ActionName("GetClientsByDay")]
        public IActionResult GetClientsByDay(Day day)
        {
            try
            {
                IEnumerable<Client> clients = _workContainer.Client.GetAll(x => x.DeliveryDay == day);

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
        [ActionName("GetClientsNotVisited")]
        public IActionResult GetClientsNotVisited(string dateFromString, string dateToString, string dealerID)
        {
            try
            {
                ApplicationUser dealer = _workContainer.ApplicationUser.GetFirstOrDefault(x => x.Id == dealerID) ?? throw new Exception("No se ha encontrado el repartidor");
                DateTime dateFrom = DateTime.Parse(dateFromString);
                DateTime dateTo = DateTime.Parse(dateToString);
                IEnumerable<Client> clients = _workContainer.Client.GetNotVisited(dateFrom, dateTo, dealer.Id);

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
