using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using AguasNico.Models.ViewModels.Routes;
using AguasNico.Models.ViewModels.Routes.Details;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
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

        #region Views

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var sessionUser = User.Identity ?? throw new Exception("No se pudo obtener el usuario de la sesión");
                var user = await _workContainer.ApplicationUser.GetFirstOrDefaultAsync(x => x.UserName != null && x.UserName.Equals(sessionUser.Name));
                var role = _signInManager.UserManager.GetRolesAsync(user).Result.First();

                IndexViewModel viewModel = new()
                {
                    User = user
                };
                var today = (Day)(int)DateTime.UtcNow.AddHours(-3).DayOfWeek;
                switch (role)
                {
                    case Constants.Admin:
                        viewModel.Routes = await _workContainer.Route.GetStaticsByDay(today);
                        return View("~/Views/Routes/Admin/Index.cshtml", viewModel);
                    case Constants.Dealer:
                        viewModel.Routes = await _workContainer.Route.GetStaticsByDealer(user.Id);
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
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> Create()
        {
            try
            {
                CreateViewModel viewModel = new()
                {
                    Dealers = await _workContainer.ApplicationUser.GetDealers(),
                    Route = new()
                };
                return View("~/Views/Routes/Admin/Create.cshtml", viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            try
            {
                var sessionUser = User.Identity ?? throw new Exception("No se pudo obtener el usuario de la sesión");
                var user = await _workContainer.ApplicationUser.GetFirstOrDefaultAsync(x => x.UserName != null && x.UserName.Equals(sessionUser.Name));
                var role = _signInManager.UserManager.GetRolesAsync(user).Result.First();

                var route = await _workContainer.Route.GetFirstOrDefaultAsync(x => x.ID == id, includeProperties: "User, Carts, Carts.Products, Carts.Products, Carts.AbonoProducts, Carts.ReturnedProducts, Carts.Client, Carts.PaymentMethods, Carts.PaymentMethods.PaymentMethod");

                if (route is null)
                    return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Error al obtener la planilla\nLa planilla no existe", ErrorCode = 404 });
                
                if ((route.UserID != user.Id && role != Constants.Admin) || (route.IsStatic && role != Constants.Admin))
                    return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Error al obtener la planilla\nNo tienes permisos para ver esta planilla", ErrorCode = 403 });

                var clientIDs = route.Carts.Select(x => x.ClientID).Distinct().ToList();

                var clientData = new List<ClientData>();
                foreach (var clientID in clientIDs)
                {
                    clientData.Add(await GetProductsAndAbono(clientID));
                }
                var states = new List<State>();
                foreach (var state in new ConstantsMethods().GetStates())
                {
                    if (state == State.Pending || state == State.Confirmed)
                        continue;
                    states.Add(state);
                }

                switch (role)
                {
                    case Constants.Admin:
                        var completedCarts = await _workContainer.Cart.GetAllAsync(x => x.RouteID == id && x.State != State.Pending);
                        var pendingCarts = await _workContainer.Cart.GetAllAsync(x => x.RouteID == id && x.State == State.Pending);

                        AdminViewModel adminViewModel = new()
                        {
                            Route = route,
                            Clients = clientData,
                            TotalExpenses = await _workContainer.Expense.GetTotalExpensesByDealer(route.CreatedAt.Date, route.UserID),
                            TotalSold = await _workContainer.Route.GetTotalSoldByRoute(id),
                            CompletedCarts = completedCarts.Count(),
                            PendingCarts = pendingCarts.Count(),
                            SoldProducts = await _workContainer.Tables.GetSoldProductsByRoute(id),
                            Payments = await _workContainer.Route.GetTotalCollected(route.ID),
                            Transfers = await _workContainer.Transfer.GetAllAsync(x => x.UserID == route.UserID && x.Date.Date == route.CreatedAt.Date, includeProperties: "Client"),
                            PaymentTypes = await _workContainer.PaymentMethod.GetFilterDropDownList(),
                            States = states,
                        };
                        return View($"~/Views/Routes/Admin/{(route.IsStatic ? "StaticDetails" : "Details")}.cshtml", adminViewModel);

                    case Constants.Dealer:

                        DealerViewModel dealerViewModel = new()
                        {
                            Route = route,
                            Clients = clientData,
                            PaymentMethods = await _workContainer.PaymentMethod.GetDropDownList(),
                            PaymentTypes = await _workContainer.PaymentMethod.GetFilterDropDownList(),
                            States = states,
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

        private async Task<ClientData> GetProductsAndAbono(long id)
        {
            try
            {
                var client = await _workContainer.Client.GetFirstOrDefaultAsync(x => x.ID == id, includeProperties: "Products, Products.Product");

                var abonoProductsList = await _workContainer.Client.GetAbonosRenewedAvailables(id);

                var products = new List<ClientData.Product>();
                var abonoProducts = abonoProductsList.GroupBy(x => x.Type).Select(x => new ClientData.Product()
                {
                    Type = x.Key,
                    Name = x.Key.GetDisplayName(),
                    Available = x.Sum(y => y.Available),
                })
                .ToList();

                foreach (var clientProduct in client.Products)
                {
                    if (clientProduct.Product.Type == ProductType.Máquina)
                        continue;

                    products.Add(new ClientData.Product()
                    {
                        Type = clientProduct.Product.Type,
                        Name = clientProduct.Product.Name,
                        Price = clientProduct.Product.Price,
                    });
                }

                return new ClientData()
                {
                    ClientID = id,
                    Products = products,
                    AbonoProducts = abonoProducts,
                };
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var route = await _workContainer.Route.GetFirstOrDefaultAsync(x => x.ID == id, includeProperties: "User, Carts, Carts.Client");
                
                if (route is null)
                    return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Error al obtener la planilla\nLa planilla no existe", ErrorCode = 404 });

                EditViewModel viewModel = new()
                {
                    Route = route,
                    ClientsInRoute = await _workContainer.Route.ClientsInRoute(id),
                    ClientsNotInRoute = await _workContainer.Route.ClientsNotInRoute(id),
                };
                return View("~/Views/Routes/Admin/Edit.cshtml", viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ManualCart(long id)
        {
            try
            {
                var route = await _workContainer.Route.GetFirstOrDefaultAsync(x => x.ID == id, includeProperties: "User, Carts") ?? throw new Exception("La planilla no existe");
                
                var sessionUser = User.Identity ?? throw new Exception("No se pudo obtener el usuario de la sesión");
                var user = await _workContainer.ApplicationUser.GetFirstOrDefaultAsync(x => x.UserName != null && x.UserName.Equals(sessionUser.Name));
                var role = _signInManager.UserManager.GetRolesAsync(user).Result.First();

                if (route.IsStatic)
                    return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Error al obtener la planilla\nLa planilla no existe", ErrorCode = 404 });
                
                if (route.UserID != user.Id && role != Constants.Admin)
                    return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Error al obtener la planilla\nNo tienes permisos para ver esta planilla", ErrorCode = 403 });

                ManualCartViewModel viewModel = new()
                {
                    Route = route,
                    Clients = await _workContainer.Route.ClientsNotInRoute(id),
                    PaymentMethods = await _workContainer.PaymentMethod.GetDropDownList()
                };
                return View(viewModel);
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
        public async Task<IActionResult> Create(Models.Route route)
        {
            ModelState.Remove("route.Carts");
            ModelState.Remove("route.User");
            ModelState.Remove("route.DispatchedProducts");
            if (ModelState.IsValid)
            {
                try
                {
                    route.IsStatic = true;
                    if (await _workContainer.Route.GetFirstOrDefaultAsync(x => x.DayOfWeek == route.DayOfWeek && x.IsStatic && x.UserID == route.UserID) is not null)
                        return CustomBadRequest(title: "Error al crear la planilla", message: "El repartidor ya tiene una planilla para ese día");
                    
                    await _workContainer.Route.AddAsync(route);
                    await _workContainer.SaveAsync();

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateByDealer(long routeID)
        {
            try
            {
                var id = await _workContainer.Route.CreateByDealer(routeID);

                return Json(new
                {
                    success = true,
                    id,
                    message = "La planilla se creó correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al crear la planilla", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> UpdateClients(Models.Route route, List<Client> clients)
        {
            try
            {
                await _workContainer.Route.UpdateClients(route.ID, clients);
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
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> SoftDelete(Models.Route route)
        {
            try
            {
                await _workContainer.Route.SoftDelete(route.ID);
                return Json(new
                {
                    success = true,
                    message = "La planilla se eliminó correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error", message: "No se ha podido eliminar la planilla", error: e.Message);
            }
        }

        #endregion

        #region AJAX

        [HttpGet]
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> SearchByDate(string dateString)
        {
            try
            {
                var date = DateTime.Parse(dateString);
                var routes = await _workContainer.Route.GetAllAsync(x => x.CreatedAt.Date == date.Date && !x.IsStatic, includeProperties: "User, Carts, Carts.PaymentMethods");

                return Json(new
                {
                    success = true,
                    routes = routes.Select(x => new
                    {
                        id = x.ID,
                        dealer = x.User.UserName,
                        totalCarts = x.Carts.Count(),
                        completedCarts = x.Carts.Count(y => y.State != State.Pending),
                        state = x.Carts.Count(y => y.State != State.Pending) == x.Carts.Count() ? "Completado" : "Pendiente",
                        collected = x.Carts.Sum(y => y.PaymentMethods.Where(z => z.CreatedAt.Date == date.Date).Sum(z => z.Amount)) != 0 ? x.Carts.Sum(y => y.PaymentMethods.Where(z => z.CreatedAt.Date == date.Date).Sum(z => z.Amount)).ToString("#,##") : "0",

                    })
                });
            }
            catch (Exception)
            {
                return CustomBadRequest(title: "No se encontraron planillas", message: "Intente nuevamente o comuníquese para soporte");
            }
        }

        [HttpGet]
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> SearchSoldProducts(string dateString, long? routeID = null)
        {
            try
            {
                var date = DateTime.Parse(dateString);
                return routeID switch
                {
                    null => Json(new
                    {
                        success = true,
                        data = await _workContainer.Tables.GetSoldProductsByDate(date)
                    }),
                    _ => Json(new
                    {
                        success = true,
                        data = await _workContainer.Tables.GetSoldProductsByDateAndRoute(date, routeID.Value)
                    }),
                };
            }
            catch (Exception)
            {
                return CustomBadRequest(title: "No se encontraron planillas", message: "Intente nuevamente o comuníquese para soporte");
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchByDay(Day dayString)
        {
            try
            {
                var sessionUser = User.Identity ?? throw new Exception("No se pudo obtener el usuario de la sesión");
                var user = await _workContainer.ApplicationUser.GetFirstOrDefaultAsync(x => x.UserName != null && x.UserName.Equals(sessionUser.Name));
                var role = _signInManager.UserManager.GetRolesAsync(user).Result.First();

                switch (role)
                {
                    case Constants.Admin:
                    {

                        break;
                    }

                    default:
                        break;
                }

                switch (role)
                {
                    case Constants.Admin:
                    {
                        var routes = await _workContainer.Route.GetAllAsync(x => x.DayOfWeek == dayString && x.IsStatic, includeProperties: "User, Carts");
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
                    case Constants.Dealer:
                        {
                            var routes = await _workContainer.Route.GetAllAsync(x => x.DayOfWeek == dayString && !x.IsStatic && x.UserID == user.Id, includeProperties: "User, Carts, Carts.PaymentMethods");
                            return Json(new
                            {
                                success = true,
                                routes = routes.OrderByDescending(x => x.CreatedAt)
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
                            });
                        }
                    default:
                        return CustomBadRequest(title: "No se encontraron planillas", message: "Intente nuevamente o comuníquese para soporte");
                };
            }
            catch (Exception)
            {
                return CustomBadRequest(title: "No se encontraron planillas", message: "Intente nuevamente o comuníquese para soporte");
            }
        }

        #endregion

        #region Details actions

        [HttpGet]
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> GetDispatched(long routeID)
        {
            try
            {
                var dispatchedProducts = await _workContainer.DispatchedProduct.GetAllAsync(x => x.RouteID == routeID);
                
                var data = new List<object>();
                foreach (var productType in new ConstantsMethods().GetProductTypes())
                {
                    if (productType == ProductType.Máquina)
                        continue;

                    data.Add(new
                    {
                        type = productType,
                        name = productType.GetDisplayName(),
                        quantity = dispatchedProducts.Any(x => x.Type == productType) ? dispatchedProducts.Where(x => x.Type == productType).First().Quantity : 0,
                    });
                }
                return Json(new
                {
                    success = true,
                    data
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al obtener los productos", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> UpdateDispatched(long routeID, List<DispatchedProduct> products)
        {
            try
            {
                await _workContainer.Route.UpdateDispatched(routeID, products);
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
