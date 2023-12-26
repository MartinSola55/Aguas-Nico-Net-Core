using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using AguasNico.Models.ViewModels.Dealers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace AguasNico.Controllers
{
    [Authorize]
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
        public IActionResult Details()
        {
            try
            {
                ApplicationUser dealer = _workContainer.ApplicationUser.GetFirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
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
    }
}
