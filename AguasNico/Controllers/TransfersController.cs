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
        [ActionName("Index")]
        public IActionResult Index()
        {
            try
            {
                IndexViewModel viewModel = new()
                {
                    Transfers = _workContainer.Transfer.GetAll(x => x.CreatedAt.Date == DateTime.UtcNow.AddHours(-3).Date, includeProperties: "Client, User").OrderByDescending(x => x.Date).ThenByDescending(x => x.Amount),
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
            ModelState.Remove("CreateViewModel.User");
            ModelState.Remove("CreateViewModel.Client");
            ModelState.Remove("CreateViewModel.UserID");
            if (ModelState.IsValid)
            {
                try
                {
                    Transfer transfer = viewModel.CreateViewModel;

                    transfer.CreatedAt = DateTime.UtcNow.AddHours(-3);
                    _workContainer.Transfer.Add(transfer);
                    _workContainer.Save();

                    Transfer newTransfer = _workContainer.Transfer.GetFirstOrDefault(x => x.ID == transfer.ID, includeProperties: "Client, User");

                    object data = new
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
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(IndexViewModel viewModel)
        {
            ModelState.Remove("CreateViewModel.User");
            ModelState.Remove("CreateViewModel.Client");
            ModelState.Remove("CreateViewModel.UserID");
            ModelState.Remove("CreateViewModel.ClientID");
            if (ModelState.IsValid)
            {
                try
                {
                    Transfer transfer = viewModel.CreateViewModel;
                    _workContainer.Transfer.Update(transfer);

                    Transfer newTransfer = _workContainer.Transfer.GetFirstOrDefault(x => x.ID == transfer.ID, includeProperties: "Client, User");

                    object data = new
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
        [ActionName("SoftDelete")]
        [ValidateAntiForgeryToken]
        public IActionResult SoftDelete(long id)
        {
            try
            {
                Transfer transfer = _workContainer.Transfer.GetOne(id) ?? throw new Exception("No se encontró la transferencia");
                _workContainer.Transfer.SoftDelete(id);
                _workContainer.Save();

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
