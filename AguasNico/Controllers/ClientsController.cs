using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using AguasNico.Models.ViewModels.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        [HttpGet]
        [ActionName("Index")]
        [Authorize(Roles = Constants.Admin)]
        public IActionResult Index()
        {
            try
            {
                IndexViewModel viewModel = new()
                {
                    Clients = _workContainer.Client.GetAll(x => x.IsActive, includeProperties: "Dealer").OrderBy(x => x.Name),
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
                ApplicationUser user = _workContainer.ApplicationUser.GetFirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
                string role = _signInManager.UserManager.GetRolesAsync(user).Result.First();
                CreateViewModel viewModel = new()
                {
                    Role = role,
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
        [Authorize(Roles = Constants.Admin)]
        public IActionResult Details(long id)
        {
            try
            {
                Client client = _workContainer.Client.GetFirstOrDefault(x => x.ID == id, includeProperties: "Dealer, Products");
                if (client is null)
                {
                    return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "El cliente no existe", ErrorCode = 404 });
                }
                

                DetailsViewModel viewModel = new()
                {
                    Client = client,
                    Dealers = _workContainer.ApplicationUser.GetDealersDropDownList(),
                    Products = _workContainer.Client.GetAllProducts(id),
                    CartsTransfersHistory = _workContainer.Client.GetCartsTransfersHistoryTable(id),
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
            ModelState.Remove("Role");
            ModelState.Remove("Client.Carts");
            ModelState.Remove("Client.Dealer");
            ModelState.Remove("Client.Transfers");
            ModelState.RemoveAll<CreateViewModel>(x => x.Client.Products);
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
        [Authorize(Roles = Constants.Admin)]
        public IActionResult Edit(Client client)
        {
            ModelState.Remove("Client.Carts");
            ModelState.Remove("Client.Dealer");
            ModelState.Remove("Client.Transfers");
            ModelState.Remove("Client.InvoiceType");
            ModelState.Remove("Client.TaxCondition");
            ModelState.Remove("Client.CUIT");
            ModelState.Remove("Client.Products");
            if (ModelState.IsValid)
            {
                try
                {
                    _workContainer.Client.Update(client);

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
        [ActionName("UpdateInvoiceData")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
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
        [Authorize(Roles = Constants.Admin)]
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
                return CustomBadRequest(title: "Error al actualizar los productos", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }
    
        [HttpPost]
        [ActionName("SoftDelete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
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


        #region Pegadas AJAX

        [HttpGet]
        [ActionName("SearchByName")]
        public IActionResult SearchByName(string name)
        {
            try
            {
                IEnumerable<Client> clients = _workContainer.Client.GetAll(x => x.Name.Contains(name), includeProperties: "Dealer");
                List<object> clientsList = [];
                foreach (Client client in clients)
                {
                    clientsList.Add(new
                    {
                        id = client.ID,
                        name = client.Name,
                        address = client.Address,
                        dealer = client.Dealer is not null ? client.Dealer.UserName : "",
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
        [ActionName("GetProducts")]
        public IActionResult GetProducts(long id)
        {
            try
            {
                Client client = _workContainer.Client.GetFirstOrDefault(x => x.ID == id, includeProperties: "Products, Products.Product");
                if (client is null) return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "El cliente no existe", ErrorCode = 404 });

                List<object> products = [];
                foreach (ClientProduct clientProduct in client.Products)
                {
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
                    data = products,
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al obtener los productos", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpGet]
        [ActionName("GetProductsHistory")]
        public IActionResult GetProductsHistory(long id)
        {
            try
            {
                Client client = _workContainer.Client.GetOne(id) ?? throw new Exception("El cliente no existe");
                List<object> data = [];
                foreach (ProductHistory productHistory in _workContainer.Client.GetProductsHistory(id))
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
