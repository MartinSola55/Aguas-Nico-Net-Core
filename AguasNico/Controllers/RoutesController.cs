using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using AguasNico.Models.ViewModels.Routes;
using AguasNico.Models.ViewModels.Routes.Details;
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
                return View("~/Views/Routes/Admin/Create.cshtml", viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Models.Route route)
        {
            ModelState.Remove("route.Carts");
            ModelState.Remove("route.User");
            if (ModelState.IsValid)
            {
                try
                {
                    route.IsStatic = true;
                    if (_workContainer.Route.GetFirstOrDefault(x => x.DayOfWeek == route.DayOfWeek && x.IsStatic && x.UserID == route.UserID) is not null)
                    {
                        return CustomBadRequest(title: "Error al crear la planilla", message: "El repartidor ya tiene una planilla para ese día");
                    }
                    _workContainer.Route.Add(route);
                    
                    _workContainer.Save();

                    return Json(new
                    {
                        success = true,
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
                Models.Route route = _workContainer.Route.GetFirstOrDefault(x => x.ID == id, includeProperties: "User, Carts, Carts.Products, Carts.ReturnedProducts, Carts.Client, Carts.PaymentMethods, Carts.PaymentMethods.PaymentMethod");
                
                if (route is null) return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Error al obtener la planilla\nLa planilla no existe", ErrorCode = 404 });
                if (route.UserID != user.Id && role != Constants.Admin) return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Error al obtener la planilla\nNo tiene permisos para ver esta planilla", ErrorCode = 403 });

                switch (role)
                {
                    case Constants.Admin:
                        AdminViewModel adminViewModel = new()
                        {
                            Route = route,
                            TotalExpenses = _workContainer.Expense.GetTotalExpensesByDealer(DateTime.UtcNow.AddHours(-3).Date, route.UserID),
                            TotalSold = _workContainer.Route.GetTotalSoldByRoute(DateTime.UtcNow.AddHours(-3).Date, id),
                            CompletedCarts = _workContainer.Cart.GetAll(x => x.RouteID == id && x.State != State.Pending).Count(),
                            PendingCarts = _workContainer.Cart.GetAll(x => x.RouteID == id && x.State == State.Pending).Count(),
                            SoldProducts = _workContainer.Tables.GetSoldProductsByRoute(id),
                            Payments = _workContainer.Route.GetTotalCollected(route.ID),
                            Transfers = _workContainer.Transfer.GetAll(x => x.UserID == route.UserID && x.Date.Date == route.CreatedAt.Date),
                        };
                        return View("~/Views/Routes/Admin/Details.cshtml", adminViewModel);
                    case Constants.Dealer:
                        DealerViewModel dealerViewModel = new()
                        {
                            Route = route,
                        };
                        return View("~/Views/Routes/Dealer/Details.cshtml", dealerViewModel);
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
        [ActionName("Edit")]
        public IActionResult Edit(long id)
        {
            try
            {
                Models.Route route = _workContainer.Route.GetFirstOrDefault(x => x.ID == id, includeProperties: "User, Carts, Carts.Client");
                if (route is null) return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Error al obtener la planilla\nLa planilla no existe", ErrorCode = 404 });

                EditViewModel viewModel = new()
                {
                    Route = route,
                    ClientsInRoute = _workContainer.Route.ClientsInRoute(id),
                    ClientsNotInRoute = _workContainer.Route.ClientsNotInRoute(id),
                };
                return View("~/Views/Routes/Admin/Edit.cshtml", viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        [HttpPost]
        [ActionName("UpdateClients")]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateClients(Models.Route route, List<Client> clients)
        {
            try
            {
                _workContainer.Route.UpdateClients(route.ID, clients);
                _workContainer.Save();
                return Json(new
                {
                    success = true,
                    id = route.ID,
                    message = "La planilla se actualizó correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al actualizar la planilla", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
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

        #region Pegadas AJAX
        [HttpGet]
        [ActionName("SearchByDate")]
        public IActionResult SearchByDate(string dateString)
        {
            try
            {
                DateTime date = DateTime.Parse(dateString);
                IEnumerable<Models.Route> routes = _workContainer.Route.GetAll(x => x.CreatedAt.Date == date.Date && !x.IsStatic, includeProperties: "User, Carts, Carts.PaymentMethods");

                return Json(new
                {
                    success = true,
                    routes = routes.Select(x => new {
                        id = x.ID,
                        dealer = x.User.UserName,
                        totalCarts = x.Carts.Count(),
                        completedCarts = x.Carts.Count(y => y.State == State.Confirmed),
                        state = x.Carts.Count(y => y.State != State.Pending) == x.Carts.Count() ? "Completado" : "Pendiente",
                        collected = x.Carts.Sum(y => y.PaymentMethods.Where(z => z.CreatedAt.Date == date.Date).Sum(z => z.Amount)),
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
                ApplicationUser user = _workContainer.ApplicationUser.GetFirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
                string role = _signInManager.UserManager.GetRolesAsync(user).Result.First();
                return role switch
                {
                    Constants.Admin => Json(new
                    {
                        success = true,
                        routes = _workContainer.Route.GetAll(x => x.DayOfWeek == dayString && x.IsStatic, includeProperties: "User, Carts").Select(x => new
                        {
                            id = x.ID,
                            dealer = x.User.UserName,
                            totalCarts = x.Carts.Count(),
                        })
                    }),
                    Constants.Dealer => Json(new
                    {
                        success = true,
                        routes = _workContainer.Route.GetAll(x => x.DayOfWeek == dayString && !x.IsStatic, includeProperties: "User, Carts, Carts.PaymentMethods")
                        .OrderByDescending(x => x.CreatedAt)
                        .Select(x => new
                        {
                            id = x.ID,
                            dealer = x.User.UserName,
                            totalCarts = x.Carts.Count(),
                            completedCarts = x.Carts.Count(y => y.State != State.Pending),
                            state = x.Carts.Count(y => y.State != State.Pending) == x.Carts.Count() ? "Completado" : "Pendiente",
                            totalCollected = x.Carts.Sum(y => y.PaymentMethods.Sum(z => z.Amount)),
                            date = x.CreatedAt.ToString("dd/MM/yyyy"),
                        })
                    }),
                    _ => CustomBadRequest(title: "No se encontraron planillas", message: "Intente nuevamente o comuníquese para soporte"),
                };
            }
            catch (Exception)
            {
                return CustomBadRequest(title: "No se encontraron planillas", message: "Intente nuevamente o comuníquese para soporte");
            }
        }

        #endregion

        #region Details actions

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
