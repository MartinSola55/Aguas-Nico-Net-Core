using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using AguasNico.Models.ViewModels.Routes;
using AguasNico.Models.ViewModels.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Globalization;
using System.Linq.Expressions;

namespace AguasNico.Controllers
{
    [Authorize]
    public class RoutesController(IWorkContainer workContainer, SignInManager<ApplicationUser> signInManager) : Controller
    {
        private readonly IWorkContainer _workContainer = workContainer;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
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
                ApplicationUser user = _workContainer.ApplicationUser.GetFirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
                string role = _signInManager.UserManager.GetRolesAsync(user).Result.First();
                IndexViewModel viewModel = new()
                {
                    User = user
                };
                Day today = (Day)(int)DateTime.UtcNow.AddHours(-3).DayOfWeek;
                switch (role)
                {
                    case Constants.Admin:
                        viewModel.Routes = _workContainer.Route.GetStaticsByDay(today);
                        return View("~/Views/Routes/Admin/Index.cshtml", viewModel);
                    case Constants.Dealer:
                        viewModel.Routes = _workContainer.Route.GetStaticsByDay(today, user.Id);
                        return View("~/Views/Routes/Dealer/Index.cshtml", viewModel);
                    default:
                        return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
                }
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        [HttpGet]
        [ActionName("Create")]
        public IActionResult Create()
        {
            try
            {
                CreateViewModel viewModel = new()
                {
                    Dealers = _workContainer.ApplicationUser.GetDealers(),
                    Route = new()
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
        public IActionResult Create(CreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Models.Route route = viewModel.Route;

                    route.IsStatic = true;
                    route.CreatedAt = DateTime.UtcNow.AddHours(-3);
                    _workContainer.Route.Add(route);
                    
                    // TODO: Agregar productos despachados

                    _workContainer.Save();

                    Models.Route newRoute = _workContainer.Route.GetOne(route.ID);
                    return Json(new
                    {
                        success = true,
                        data = newRoute,
                        message = "La planilla se creó correctamente",
                    });
                }
                catch (Exception e)
                {
                    return CustomBadRequest(title: "Error al crear la planilla", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
                }
            }
            return CustomBadRequest(title: "Error al crear la planilla", message: "Alguno de los campos ingresados no es válido");
        }

        [HttpGet]
        [ActionName("Details")]
        public IActionResult Details(long id)
        {
            try
            {
                ApplicationUser user = _workContainer.ApplicationUser.GetFirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
                string role = _signInManager.UserManager.GetRolesAsync(user).Result.First();
                Models.Route route = _workContainer.Route.GetFirstOrDefault(x => x.ID == id, includeProperties: "User, Carts, Carts.Client, Carts.CartPaymentMethod, Carts.CartPaymentMethod.PaymentMethod");
                if (route is null)
                {
                    return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Error al obtener la planilla\nLa planilla no existe", ErrorCode = 404 });
                }

                DetailsViewModel viewModel = new()
                {
                    Route = route,
                    PaymentMethods = _workContainer.PaymentMethod.GetDropDownList(),
                };

                if (role == Constants.Admin)
                {
                    viewModel.DispatchedProducts = _workContainer.DispatchedProduct.GetAllFromRoute(id).OrderBy(x => x.Type);
                    // TODO: Get stats
                }
                return View(viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(long id)
        {
            try
            {
                if (_workContainer.Route.GetOne(id) is null)
                {
                    return CustomBadRequest(title: "Error al eliminar la planilla", message: "La planilla no existe");
                }
                _workContainer.Route.SoftDelete(id);
                _workContainer.Save();
                return Json(new
                {
                    success = true,
                    message = "La planilla se eliminó correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al eliminar la planilla", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpGet]
        [ActionName("SearchByDate")]
        public IActionResult SearchByDate(string dateString)
        {
            try
            {
                DateTime date = DateTime.Parse(dateString);
                IEnumerable<Models.Route> routes = _workContainer.Route.GetAll(x => x.CreatedAt.Date == date.Date && x.IsStatic, includeProperties: "User, Carts, Carts.PaymentMethods");

                return Json(new
                {
                    success = true,
                    routes = routes.Select(x => new {
                        id = x.ID,
                        dealer = x.User.UserName,
                        totalCarts = x.Carts.Count(),
                        completedCarts = x.Carts.Count(y => y.State == State.Confirmed),
                        state = x.Carts.Count(y => y.State != State.Pending) == x.Carts.Count() ? "Completado" : "Pendiente",
                        collected = x.Carts.Sum(y => y.PaymentMethods.Sum(z => z.Amount)),
                    })
                });
            }
            catch (Exception)
            {
                return CustomBadRequest(title: "No se encontraron planillas", message: "Intente nuevamente o comuníquese para soporte");
            }
        }

        [HttpGet]
        [ActionName("SearchSoldProducts")]
        public IActionResult SearchSoldProducts(string dateString, long? routeID = null)
        {
            try
            {
                DateTime date = DateTime.Parse(dateString);
                return routeID switch
                {
                    null => Json(new
                    {
                        success = true,
                        data = _workContainer.Tables.GetSoldProductsByDate(date)
                    }),
                    _ => Json(new
                    {
                        success = true,
                        data = _workContainer.Tables.GetSoldProductsByDateAndRoute(date, routeID.Value)
                    }),
                };
            }
            catch (Exception)
            {
                return CustomBadRequest(title: "No se encontraron planillas", message: "Intente nuevamente o comuníquese para soporte");
            }
        }

        [HttpGet]
        [ActionName("SearchByDay")]
        public IActionResult SearchByDay(Day dayString)
        {
            try
            {
                IEnumerable<Models.Route> routes = _workContainer.Route.GetAll(x => x.DayOfWeek == dayString && x.IsStatic, includeProperties: "User, Carts");

                return Json(new
                {
                    success = true,
                    routes = routes.Select(x => new
                    {
                        id = x.ID,
                        dealer = x.User.UserName,
                        totalCarts = x.Carts.Count(),
                    })
                });
            }
            catch (Exception)
            {
                return CustomBadRequest(title: "No se encontraron planillas", message: "Intente nuevamente o comuníquese para soporte");
            }
        }

        #region Route Details Actions

        [HttpPost]
        [ActionName("UpdateClients")] // For admin. Deletes and creates all static carts.
        public IActionResult UpdateClients(long routeID, List<Client> clients)
        {
            try
            {
                Models.Route route = _workContainer.Route.GetOne(routeID);
                if (route is null)
                {
                    return CustomBadRequest(title: "Error al actualizar los clientes", message: "La planilla no existe");
                }
                _workContainer.Route.UpdateClients(routeID, clients);
                _workContainer.Save();
                return Json(new
                {
                    success = true,
                    message = "Los clientes se actualizaron correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al actualizar los clientes", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpPost]
        [ActionName("AddClient")] // For employee. Adds a new cart.
        public IActionResult AddClient(long routeID, long clientID)
        {
            try
            {
                Models.Route route = _workContainer.Route.GetOne(routeID);
                if (route is null)
                {
                    return CustomBadRequest(title: "Error al obtener la planilla", message: "La planilla no existe");
                }
                Client client = _workContainer.Client.GetOne(clientID);
                if (client is null)
                {
                    return CustomBadRequest(title: "Error al obtener el cliente", message: "El cliente no existe");
                }
                Cart cart = new()
                {
                    ClientID = clientID,
                    RouteID = routeID,
                    CreatedAt = DateTime.UtcNow.AddHours(-3),
                    IsStatic = false,
                    Priority = route.Carts.Max(x => x.Priority) + 1,
                };
                _workContainer.Cart.Add(cart);
                _workContainer.Save();
                return Json(new
                {
                    success = true,
                    message = "El cliente se agregó a la planilla correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al agregar el cliente", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpPost]
        [ActionName("UpdateDispatched")] // For employee. Adds a new cart.
        public IActionResult UpdateDispatched(long routeID, List<DispatchedProduct> products)
        {
            try
            {
                Models.Route route = _workContainer.Route.GetOne(routeID);
                if (route is null)
                {
                    return CustomBadRequest(title: "Error al obtener la planilla", message: "La planilla no existe");
                }
                foreach (DispatchedProduct product in products)
                {
                    product.RouteID = routeID;
                    product.UpdatedAt = DateTime.UtcNow.AddHours(-3);
                    _workContainer.DispatchedProduct.Add(product);
                }
                return Json(new
                {
                    success = true,
                    message = "Los productos se actualizaron correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al actualizar los productos", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpPost]
        [ActionName("UpdateReturned")] // For employee. Adds a new cart.
        public IActionResult UpdateReturned()
        {
            try
            {
                return Json(new
                {
                    success = true,
                    message = "Los productos se actualizaron correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al actualizar los productos", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        #endregion
    }
}
