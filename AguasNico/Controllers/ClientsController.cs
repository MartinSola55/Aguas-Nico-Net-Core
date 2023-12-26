using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using AguasNico.Models.ViewModels.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq.Expressions;

namespace AguasNico.Controllers
{
    [Authorize]
    public class ClientsController(IWorkContainer workContainer) : Controller
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
                    Clients = _workContainer.Client.GetAll(includeProperties: "Dealer").OrderBy(x => x.Name),
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
        public IActionResult Create()
        {
            try
            {
                CreateViewModel viewModel = new()
                {
                    Products = _workContainer.Product.GetAll().OrderBy(x => x.Name).ThenByDescending(x => x.Price),
                    Dealers = _workContainer.ApplicationUser.GetDealersDropDownList(),
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
        public IActionResult Details(long id)
        {
            try
            {
                Client client = _workContainer.Client.GetFirstOrDefault(x => x.ID == id, includeProperties: "Dealer, ClientProducts");
                if (client is null)
                {
                    return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "El cliente no existe", ErrorCode = 404 });
                }
                DetailsViewModel viewModel = new()
                {
                    Client = client,
                    Dealers = _workContainer.ApplicationUser.GetDealersDropDownList(),
                    Transfers = _workContainer.Transfer.GetLastTen(id),
                    Carts = _workContainer.Cart.GetLastTen(id),
                    Products = _workContainer.Client.GetAllProducts(id),
                    ProductsHistory = _workContainer.Client.GetProductsHistory(id),
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
            ModelState.Remove("Client.Carts");
            ModelState.Remove("Client.Dealer");
            ModelState.Remove("Client.Transfers");
            ModelState.RemoveAll<CreateViewModel>(x => x.Client.ClientProducts);
            if (ModelState.IsValid)
            {
                try
                {
                    Client client = viewModel.Client;
                    _workContainer.BeginTransaction();
                    _workContainer.Client.Add(client);
                    _workContainer.Save();
                    
                    if (client.DealerID is not null && client.DeliveryDay is not null)
                    {
                        Models.Route route = _workContainer.Route.GetFirstOrDefault(x => x.UserID == client.DealerID && x.DayOfWeek == client.DeliveryDay, includeProperties: "Carts");
                        if (route is not null)
                        {
                            int priority = route.Carts.Any() ? route.Carts.Max(x => x.Priority) + 1 : 1;
                            Cart cart = new()
                            {
                                RouteID = route.ID,
                                ClientID = client.ID,
                                Priority = priority,
                                State = State.Pending,
                                IsStatic = true,
                            };
                            _workContainer.Cart.Add(cart);
                        }
                    }
                    _workContainer.Save();
                    _workContainer.Commit();

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
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Client client)
        {
            ModelState.Remove("Client.Carts");
            ModelState.Remove("Client.Dealer");
            ModelState.Remove("Client.Transfers");
            ModelState.Remove("Client.InvoiceType");
            ModelState.Remove("Client.TaxCondition");
            ModelState.Remove("Client.CUIT");
            ModelState.Remove("Client.ClientProducts");
            if (ModelState.IsValid)
            {
                try
                {
                    _workContainer.BeginTransaction();
                    _workContainer.Client.Update(client);
                    Cart cart = _workContainer.Cart.GetFirstOrDefault(x => x.ClientID == client.ID && x.IsStatic);
                    if (cart is not null)
                    {
                        cart.DeletedAt = DateTime.UtcNow.AddHours(-3);
                        _workContainer.Cart.Update(cart);
                    }
                    Models.Route route = _workContainer.Route.GetFirstOrDefault(x => x.UserID == client.DealerID && x.DayOfWeek == client.DeliveryDay && x.IsStatic, includeProperties: "Carts");
                    if (route is not null)
                    {
                        int priority = route.Carts.Any() ? route.Carts.Max(x => x.Priority) + 1 : 1;
                        cart = new()
                        {
                            RouteID = route.ID,
                            ClientID = client.ID,
                            Priority = priority,
                            State = State.Pending,
                            IsStatic = true,
                        };
                        _workContainer.Cart.Add(cart);
                    }
                    _workContainer.Save();
                    _workContainer.Commit();

                    return Json(new
                    {
                        success = true,
                        data = 1,
                        message = "El cliente se actualizó correctamente",
                    });
                }
                catch (Exception e)
                {
                    _workContainer.Rollback();
                    return CustomBadRequest(title: "Error al actualizar el cliente", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
                }
            }
            return CustomBadRequest(title: "Error al actualizar el cliente", message: "Alguno de los campos ingresados no es válido");
        }

        [HttpPost]
        [ActionName("UpdateInvoiceData")]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateInvoiceData(Client client)
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
            ModelState.Remove("Client.ClientProducts");
            if (ModelState.IsValid)
            {
                try
                {
                    _workContainer.Client.UpdateInvoiceData(client);
                    _workContainer.Save();

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
        [ActionName("UpdateProducts")]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProducts(Client client, List<ClientProduct> products)
        {
            try
            {
                _workContainer.Client.UpdateProducts(client.ID, products);
                return Json(new
                {
                    success = true,
                    data = 1,
                    message = "Los productos se actualizaron correctamente",
                });
            }
            catch (Exception e)
            {
                _workContainer.Rollback();
                return CustomBadRequest(title: "Error al actualizar los productos", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }
    
        [HttpPost]
        [ActionName("SoftDelete")]
        [ValidateAntiForgeryToken]
        public IActionResult SoftDelete(long id)
        {
            try
            {
                _workContainer.Client.SoftDelete(id);
                _workContainer.Save();
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
    }
}
