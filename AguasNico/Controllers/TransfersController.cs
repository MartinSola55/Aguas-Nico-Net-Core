using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using AguasNico.Models.ViewModels.Transfers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace AguasNico.Controllers
{
    [Authorize(Roles = Constants.Admin)]
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
        public async Task<IActionResult> Index()
        {
            try
            {
                var transfers = await _workContainer.Transfer.GetAllAsync(x => x.CreatedAt.Date == DateTime.UtcNow.AddHours(-3).Date, includeProperties: "Client, User");
                IndexViewModel viewModel = new()
                {
                    Transfers = transfers.OrderByDescending(x => x.Date).ThenByDescending(x => x.Amount),
                    Dealers = await _workContainer.ApplicationUser.GetDropDownList(),
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IndexViewModel viewModel)
        {
            ModelState.Remove("CreateViewModel.User");
            ModelState.Remove("CreateViewModel.Client");
            ModelState.Remove("CreateViewModel.UserID");
            if (ModelState.IsValid)
            {
                try
                {
                    var transfer = viewModel.CreateViewModel;

                    transfer.CreatedAt = DateTime.UtcNow.AddHours(-3);
                    await _workContainer.Transfer.AddAsync(transfer);
                    await _workContainer.SaveAsync();

                    var newTransfer = await _workContainer.Transfer.GetFirstOrDefaultAsync(x => x.ID == transfer.ID, includeProperties: "Client, User");

                    var data = new
                    {
                        id = newTransfer.ID,
                        client = newTransfer.Client.Name,
                        dealer = newTransfer.User.UserName,
                        amount = newTransfer.Amount,
                        date = newTransfer.Date,
                        createdAt = newTransfer.CreatedAt
                    };
                    return Json(new
                    {
                        success = true,
                        data,
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IndexViewModel viewModel)
        {
            ModelState.Remove("CreateViewModel.User");
            ModelState.Remove("CreateViewModel.Client");
            ModelState.Remove("CreateViewModel.UserID");
            ModelState.Remove("CreateViewModel.ClientID");
            if (ModelState.IsValid)
            {
                try
                {
                    var transfer = viewModel.CreateViewModel;
                    await _workContainer.Transfer.Update(transfer);

                    var newTransfer = await _workContainer.Transfer.GetFirstOrDefaultAsync(x => x.ID == transfer.ID, includeProperties: "Client, User");

                    var data = new
                    {
                        id = newTransfer.ID,
                        client = newTransfer.Client.Name,
                        dealer = newTransfer.User.UserName,
                        amount = newTransfer.Amount,
                        date = newTransfer.Date,
                        createdAt = newTransfer.CreatedAt
                    };
                    return Json(new
                    {
                        success = true,
                        data,
                        message = "La transferencia se editó correctamente",
                    });
                }
                catch (Exception e)
                {
                    return CustomBadRequest(title: "Error al editar la transferencia", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
                }
            }
            return CustomBadRequest(title: "Error al editar la transferencia", message: "Alguno de los campos ingresados no es válido");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoftDelete(long id)
        {
            try
            {
                var transfer = await _workContainer.Transfer.GetOneAsync(id) ?? throw new Exception("No se encontró la transferencia");
                await _workContainer.Transfer.SoftDelete(id);

                return Json(new
                {
                    success = true,
                    data = id,
                    message = "La transferencia se eliminó correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al eliminar el gasto", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchBetweenDates(string dateFrom, string dateTo)
        {
            try
            {
                var dateFromParsed = DateTime.Parse(dateFrom);
                var dateToParsed = DateTime.Parse(dateTo);

                var transfers = await _workContainer.Transfer.GetAllAsync(x => x.Date >= dateFromParsed && x.Date <= dateToParsed, includeProperties: "Client, User.UserName");
                return Json(new
                {
                    success = true,
                    data = transfers.OrderByDescending(x => x.Amount),
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al buscar las transferencias", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }
    }
}
