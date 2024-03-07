using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using AguasNico.Models.ViewModels.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace AguasNico.Controllers
{
    [Authorize]
    public class ClientsController(IWorkContainer workContainer, SignInManager<ApplicationUser> signInManager) : Controller
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
        [Authorize(Roles = Constants.Admin)]
        public IActionResult Index()
        {
            try
            {
                IndexViewModel viewModel = new();

                return View(viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var sessionUser = User.Identity ?? throw new Exception("No se pudo obtener el usuario de la sesión");
                var user = await _workContainer.ApplicationUser.GetFirstOrDefaultAsync(x => x.UserName != null && x.UserName.Equals(sessionUser.Name));
                if (user is null)
                    return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "El usuario no existe", ErrorCode = 404 });

                var role = _signInManager.UserManager.GetRolesAsync(user).Result.First();
                var products = await _workContainer.Product.GetAllAsync(x => x.IsActive);

                CreateViewModel viewModel = new()
                {
                    Role = role,
                    Products = [.. products.OrderBy(x => x.Name).ThenByDescending(x => x.Price) ],
                    Dealers = await _workContainer.ApplicationUser.GetDealersDropDownList(),
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
        public async Task<IActionResult> Details(long id)
        {
            try
            {
                var client = await _workContainer.Client.GetFirstOrDefaultAsync(x => x.ID == id, includeProperties: "Dealer, Products");
                if (client is null)
                    return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "El cliente no existe", ErrorCode = 404 });
                
                if (!client.IsActive)
                    return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "El cliente ha sido eliminado", ErrorCode = 404 });

                var abonos = await _workContainer.Abono.GetAllAsync();
                var clientAbonos = await _workContainer.Client.GetAbonos(id);

                foreach (var abono in abonos)
                {
                    if (!clientAbonos.Any(x => x.AbonoID == abono.ID))
                    {
                        clientAbonos.Add(new ClientAbono
                        {
                            Abono = abono,
                            AbonoID = abono.ID,
                        });
                    }
                }

                DetailsViewModel viewModel = new()
                {
                    Client = client,
                    Dealers = await _workContainer.ApplicationUser.GetDealersDropDownList(),
                    Products = await _workContainer.Client.GetAllProducts(id),
                    Abonos = clientAbonos,
                    CartsTransfersHistory = await _workContainer.Client.GetCartsTransfersHistoryTable(id),
                    ProductsHistory = await _workContainer.Client.GetProductsHistory(id),
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
        public async Task<IActionResult> Create(CreateViewModel viewModel)
        {
            ModelState.Remove("Role");
            ModelState.Remove("Client.Carts");
            ModelState.Remove("Client.Dealer");
            ModelState.Remove("Client.Transfers");
            ModelState.RemoveAll<CreateViewModel>(x => x.Client.Products);
            ModelState.RemoveAll<CreateViewModel>(x => x.Client.Abonos);
            ModelState.RemoveAll<CreateViewModel>(x => x.Client.AbonosRenewed);
            if (ModelState.IsValid)
            {
                try
                {
                    var client = viewModel.Client;
                    await _workContainer.Client.AddAsync(client);
                    
                    if (client.DealerID is not null && client.DeliveryDay is not null)
                    {
                        var route = await _workContainer.Route.GetFirstOrDefaultAsync(x => x.UserID == client.DealerID && x.DayOfWeek == client.DeliveryDay, includeProperties: "Carts");
                        if (route is not null)
                        {
                            int priority = route.Carts.Any() ? route.Carts.Max(x => x.Priority) + 1 : 1;
                            var cart = new Cart
                            {
                                RouteID = route.ID,
                                Client = client,
                                Priority = priority,
                                State = State.Pending,
                                IsStatic = true,
                            };
                            await _workContainer.Cart.AddAsync(cart);
                        }
                    }

                    await _workContainer.BeginTransactionAsync();
                    await _workContainer.SaveAsync();
                    await _workContainer.CommitAsync();

                    return Json(new
                    {
                        success = true,
                        data = 1,
                        message = "El cliente se guardó correctamente",
                    });
                }
                catch (Exception e)
                {
                    _workContainer.Rollback();
                    return CustomBadRequest(title: "Error al crear el cliente", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
                }
            }
            return CustomBadRequest(title: "Error al crear el cliente", message: "Alguno de los campos ingresados no es válido");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> Edit(Client client)
        {
            ModelState.Remove("Client.Carts");
            ModelState.Remove("Client.Dealer");
            ModelState.Remove("Client.Transfers");
            ModelState.Remove("Client.InvoiceType");
            ModelState.Remove("Client.TaxCondition");
            ModelState.Remove("Client.CUIT");
            ModelState.Remove("Client.Products");
            ModelState.Remove("Client.Abonos");
            ModelState.Remove("Client.AbonosRenewed");
            if (ModelState.IsValid)
            {
                try
                {
                    await _workContainer.Client.Update(client);

                    return Json(new
                    {
                        success = true,
                        message = "El cliente se actualizó correctamente",
                    });
                }
                catch (Exception e)
                {
                    return CustomBadRequest(title: "Error al actualizar el cliente", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
                }
            }
            return CustomBadRequest(title: "Error al actualizar el cliente", message: "Alguno de los campos ingresados no es válido");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> UpdateInvoiceData(Client client)
        {
            ModelState.Remove("Client.Carts");
            ModelState.Remove("Client.Dealer");
            ModelState.Remove("Client.Transfers");
            ModelState.Remove("Client.Name");
            ModelState.Remove("Client.Phone");
            ModelState.Remove("Client.Address");
            ModelState.Remove("Client.DeliveryDay");
            ModelState.Remove("Client.HasInvoice");
            ModelState.Remove("Client.DealerID");
            ModelState.Remove("Client.Products");
            ModelState.Remove("Client.Abonos");
            ModelState.Remove("Client.AbonosRenewed");
            if (ModelState.IsValid)
            {
                try
                {
                    await _workContainer.Client.UpdateInvoiceData(client);

                    return Json(new
                    {
                        success = true,
                        message = "Los datos de facturación se actualizaron correctamente",
                    });
                }
                catch (Exception e)
                {
                    return CustomBadRequest(title: "Error al actualizar los datos de facturación", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
                }
            }
            return CustomBadRequest(title: "Error al actualizar los datos de facturación", message: "Alguno de los campos ingresados no es válido");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> UpdateProducts(Client client, List<ClientProduct> products)
        {
            try
            {
                await _workContainer.Client.UpdateProducts(client.ID, products);
                return Json(new
                {
                    success = true,
                    data = 1,
                    message = "Los productos se actualizaron correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al actualizar los productos", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> UpdateAbonos(Client client, List<ClientAbono> abonos)
        {
            try
            {
                await _workContainer.Client.UpdateAbonos(client.ID, abonos);
                return Json(new
                {
                    success = true,
                    data = 1,
                    message = "Los abonos se actualizaron correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al actualizar los abonos", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> SoftDelete(long id)
        {
            try
            {
                await _workContainer.Client.SoftDelete(id);

                return Json(new
                {
                    success = true,
                    data = 1,
                    message = "El cliente se eliminó correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al eliminar el cliente", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        #endregion

        #region AJAX

        [HttpGet]
        public async Task<IActionResult> SearchByName(string name)
        {
            try
            {
                var clients = await _workContainer.Client.GetAllAsync(x => (x.Name.Contains(name) || x.Address.Contains(name)) && x.IsActive, includeProperties: "Dealer");
                var clientsList = new List<object>();
                foreach (var client in clients.OrderBy(x => x.Name))
                {
                    var day = client.DeliveryDay is not null ? client.DeliveryDay.ToString() : "Sin día asignado";
                    var dealer = client.Dealer is not null ? client.Dealer.UserName : "Sin repartidor asignado";
                    var debt = client.Debt >= 0 ? client.Debt.ToString("#,##") : (client.Debt * -1).ToString("#,##") + " a favor";
                    clientsList.Add(new
                    {
                        id = client.ID,
                        name = client.Name,
                        address = client.Address,
                        phone = client.Phone,
                        dealer = dealer + " - " + day,
                        debt = debt != "" ? debt : "0",
                    });
                }
                return Json(new
                {
                    success = true,
                    data = clientsList,
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al obtener los clientes", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsAndAbono(long id)
        {
            try
            {
                var client = await _workContainer.Client.GetFirstOrDefaultAsync(x => x.ID == id, includeProperties: "Products, Products.Product");
                
                if (client is null)
                    return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "El cliente no existe", ErrorCode = 404 });

                var abonoProductsList = await _workContainer.Client.GetAbonosRenewedAvailables(id);
                
                var products = new List<object>();
                var abonoProducts = abonoProductsList.GroupBy(x => x.Type).Select(x => new
                {
                    type = x.Key,
                    name = x.Key.GetDisplayName(),
                    available = x.Sum(y => y.Available),
                })
                .Cast<object>()
                .ToList();

                foreach (var clientProduct in client.Products)
                {
                    if (clientProduct.Product.Type == ProductType.Máquina)
                        continue;

                    products.Add(new
                    {
                        type = clientProduct.Product.Type,
                        name = clientProduct.Product.Name,
                        price = clientProduct.Product.Price,
                    });
                }

                return Json(new
                {
                    success = true,
                    products,
                    abonoProducts,
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al obtener los productos", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsHistory(long id)
        {
            try
            {
                var client = await _workContainer.Client.GetOneAsync(id) ?? throw new Exception("El cliente no existe");
                var data = new List<object>();
                foreach (var productHistory in await _workContainer.Client.GetProductsHistory(id))
                {
                    data.Add(new
                    {
                        productType = productHistory.ProductType.GetDisplayName(),
                        actionType = productHistory.ActionType.GetDisplayName(),
                        quantity = productHistory.Quantity,
                        date = productHistory.Date.ToString("dd/MM/yyyy"),
                    });
                }
                return Json(new
                {
                    success = true,
                    data,
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al obtener los productos", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }
        
        #endregion
    }
}
