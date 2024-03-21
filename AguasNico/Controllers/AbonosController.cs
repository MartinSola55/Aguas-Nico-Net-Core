using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using AguasNico.Models.ViewModels.Abonos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AguasNico.Controllers
{
    [Authorize]
    public class AbonosController(IWorkContainer workContainer, IConfiguration configuration) : BaseController(configuration)
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
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> Index()
        {
            try
            {
                IndexViewModel viewModel = new()
                {
                    Abonos = await _workContainer.Abono.GetAllAsync(includeProperties: "Products"),
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        [HttpGet]
        [Authorize(Roles = Constants.Admin)]
        public IActionResult Create()
        {
            try
            {
                return View(new CreateViewModel());
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        #endregion

        #region Actions

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> Create(CreateViewModel viewModel)
        {
            ModelState.RemoveAll<CreateViewModel>(x => x.Abono.Products);
            ModelState.RemoveAll<CreateViewModel>(x => x.Abono.Renewals);
            if (ModelState.IsValid)
            {
                try
                {
                    var abono = viewModel.Abono;
                    await _workContainer.Abono.AddAsync(abono);
                    await _workContainer.SaveAsync();

                    return Json(new
                    {
                        success = true,
                        data = 1,
                        message = "El abono se creó correctamente",
                    });
                }
                catch (Exception e)
                {
                    _workContainer.Rollback();
                    return CustomBadRequest(title: "Error al crear el abono", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
                }
            }
            return CustomBadRequest(title: "Error al crear el abono", message: "Alguno de los campos ingresados no es válido");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> Edit(IndexViewModel viewModel)
        {
            ModelState.Remove("EditedAbono.Products");
            ModelState.Remove("EditedAbono.Renewals");
            if (ModelState.IsValid)
            {
                try
                {
                    var abono = viewModel.EditedAbono;
                    await _workContainer.Abono.Update(abono);
                    await _workContainer.SaveAsync();

                    var newAbono = await _workContainer.Abono.GetOneAsync(abono.ID);
                    return Json(new
                    {
                        success = true,
                        data = newAbono,
                        message = "El abono se editó correctamente",
                    });
                }
                catch (Exception e)
                {
                    return CustomBadRequest(title: "Error al editar el abono", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
                }
            }
            return CustomBadRequest(title: "Error al editar el abono", message: "Alguno de los campos ingresados no es válido");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> SoftDelete(long id)
        {
            try
            {
                var abono = await _workContainer.Abono.GetOneAsync(id) ?? throw new Exception("No se encontró el abono");
                await _workContainer.Abono.SoftDelete(id);

                return Json(new
                {
                    success = true,
                    data = id,
                    message = "El abono se eliminó correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al eliminar el abono", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> RenewAll()
        {
            try
            {
                await _workContainer.Abono.RenewAll();

                return Json(new
                {
                    success = true,
                    message = "Todos los abonos se renovaron correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al renovar los abonos", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> RenewByRoute(long routeID)
        {
            try
            {
               await  _workContainer.Abono.RenewByRoute(routeID);

                return Json(new
                {
                    success = true,
                    message = "Los abonos se renovaron correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al renovar los abonos", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetClients(long abonoID)
        {
            try
            {
                return Json(new
                {
                    success = true,
                    data = await _workContainer.Abono.GetClients(abonoID),
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al obtener los clientes", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        #endregion
    }
}
