﻿using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using AguasNico.Models.ViewModels.Abonos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AguasNico.Controllers
{
    [Authorize]
    public class AbonosController(IWorkContainer workContainer) : Controller
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
        [Authorize(Roles = Constants.Admin)]
        public IActionResult Index()
        {
            try
            {
                IndexViewModel viewModel = new()
                {
                    Abonos = _workContainer.Abono.GetAll(includeProperties: "Products"),
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        [HttpGet]
        [ActionName("Create")]
        [Authorize(Roles = Constants.Admin)]
        public IActionResult Create()
        {
            try
            {
                List<ProductType> types = [];
                foreach (ProductType type in Enum.GetValues(typeof(ProductType)))
                {
                    types.Add(type);
                }
                CreateViewModel viewModel = new()
                {
                    Products = types
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
        [Authorize(Roles = Constants.Admin)]
        public IActionResult Create(CreateViewModel viewModel)
        {
            ModelState.RemoveAll<CreateViewModel>(x => x.Abono.Products);
            ModelState.RemoveAll<CreateViewModel>(x => x.Abono.Renewals);
            if (ModelState.IsValid)
            {
                try
                {
                    Abono abono = viewModel.Abono;
                    _workContainer.Abono.Add(abono);
                    _workContainer.Save();

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
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
        public IActionResult Edit(IndexViewModel viewModel)
        {
            ModelState.Remove("EditedAbono.Products");
            ModelState.Remove("EditedAbono.Renewals");
            if (ModelState.IsValid)
            {
                try
                {
                    Abono abono = viewModel.EditedAbono;
                    _workContainer.Abono.Update(abono);
                    _workContainer.Save();

                    Abono newAbono = _workContainer.Abono.GetOne(abono.ID);
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
        [ActionName("SoftDelete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
        public IActionResult SoftDelete(long id)
        {
            try
            {
                Abono abono = _workContainer.Abono.GetOne(id) ?? throw new Exception("No se encontró el abono");
                _workContainer.Abono.SoftDelete(id);

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
        [ActionName("RenewAll")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
        public IActionResult RenewAll()
        {
            try
            {
                _workContainer.Abono.RenewAll();

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
        [ActionName("RenewByRoute")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
        public IActionResult RenewByRoute(long routeID)
        {
            try
            {
                _workContainer.Abono.RenewByRoute(routeID);

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
        [ActionName("GetClients")]
        public IActionResult GetClients(long abonoID)
        {
            try
            {
                return Json(new
                {
                    success = true,
                    data = _workContainer.Abono.GetClients(abonoID).ToList(),
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al obtener los clientes", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }
    }
}
