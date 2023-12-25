using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using AguasNico.Models.ViewModels.Transfers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace AguasNico.Controllers
{
    [Authorize]
    public class TransfersController(IWorkContainer workContainer) : Controller
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
        public IActionResult Index()
        {
            try
            {
                Expression<Func<Transfer, bool>> filter = entity => entity.Date.Date == DateTime.UtcNow.AddHours(-3).Date;
                IndexViewModel viewModel = new()
                {
                    Transfers = _workContainer.Transfer.GetAll(filter, includeProperties: "Client, User.UserName").OrderByDescending(x => x.Date).ThenByDescending(x => x.Amount),
                    Dealers = _workContainer.ApplicationUser.GetDropDownList(),
                    CreateViewModel = new Transfer()
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IndexViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Transfer transfer = viewModel.CreateViewModel;

                    transfer.CreatedAt = DateTime.UtcNow.AddHours(-3);
                    _workContainer.Transfer.Add(transfer);
                    _workContainer.Save();

                    Transfer newTransfer = _workContainer.Transfer.GetOne(transfer.ID);
                    return Json(new
                    {
                        success = true,
                        data = newTransfer,
                        message = "La transferencia se creó correctamente",
                    });
                }
                catch (Exception e)
                {
                    return CustomBadRequest(title: "Error al crear la transferencia", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
                }
            }
            return CustomBadRequest(title: "Error al crear la transferencia", message: "Alguno de los campos ingresados no es válido");
        }

        [HttpGet]
        [ActionName("SearchBetweenDates")]
        public IActionResult SearchBetweenDates(string dateFrom, string dateTo)
        {
            try
            {
                DateTime dateFromParsed = DateTime.Parse(dateFrom);
                DateTime dateToParsed = DateTime.Parse(dateTo);
                Expression<Func<Transfer, bool>> filter = entity => entity.Date >= dateFromParsed && entity.Date <= dateToParsed;
                IEnumerable<Transfer> transfers = _workContainer.Transfer.GetAll(filter, includeProperties: "Client, User.UserName").OrderByDescending(x => x.Amount);
                return Json(new
                {
                    success = true,
                    data = transfers,
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al buscar las transferencias", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }
    }
}
