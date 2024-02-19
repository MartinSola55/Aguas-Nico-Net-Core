using AguasNico.Data.Repository.IRepository;
using AguasNico.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using AguasNico.Models;
using Microsoft.AspNetCore.Identity;
using AguasNico.Models.ViewModels.Clients;
using AguasNico.Models.ViewModels.Carts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace AguasNico.Controllers
{
    [Authorize]
    public class CartsController(IWorkContainer workContainer) : Controller
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
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var cart = await _workContainer
                    .Cart
                    .GetFirstOrDefaultAsync(x => 
                    x.ID == id,
                    includeProperties: "Route.User, Products, AbonoProducts, Client, Client.Abonos, Client.Abonos.Abono.Products, Client.Products.Product, ReturnedProducts, PaymentMethods")
                    ?? throw new Exception("No se ha encontrado la bajada solicitada");

                if (cart.State != State.Confirmed)
                    throw new Exception("No se puede editar una bajada que no esté confirmada");

                var cartProducts = new List<CartProduct>();
                var abonoProducts = new List<CartAbonoProduct>();
                var returnedProducts = new List<ReturnedProduct>();

                foreach (var clientProduct in cart.Client.Products)
                {
                    if (clientProduct.Product.Type == ProductType.Máquina) continue;
                    cartProducts.Add(new()
                    {
                        Type = clientProduct.Product.Type,
                        Quantity = cart.Products.FirstOrDefault(x => x.Type == clientProduct.Product.Type)?.Quantity ?? 0,
                        SettedPrice = cart.Products.FirstOrDefault(x => x.Type == clientProduct.Product.Type)?.SettedPrice ?? clientProduct.Product.Price,
                    });
                    returnedProducts.Add(new()
                    {
                        Type = clientProduct.Product.Type,
                        Quantity = cart.ReturnedProducts.FirstOrDefault(x => x.Type == clientProduct.Product.Type)?.Quantity ?? 0,
                    });
                }

                foreach (var product in cart.Client.Abonos.SelectMany(x => x.Abono.Products).Distinct())
                {
                    if (product.Type == ProductType.Máquina)
                        continue;

                    if (abonoProducts.Any(x => x.Type == product.Type))
                        continue;

                    abonoProducts.Add(new()
                    {
                        Type = product.Type,
                        Quantity = cart.AbonoProducts.FirstOrDefault(x => x.Type == product.Type)?.Quantity ?? 0,
                    });
                }

                var methods = await _workContainer.PaymentMethod.GetAllAsync();

                var methodsOrdered = methods.OrderBy(x => x.ID).Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.ID.ToString(),
                    Selected = cart.PaymentMethods.Any(x => x.PaymentMethodID == i.ID),
                }).ToList();

                var editViewModel = new EditViewModel()
                {
                    Cart = cart,
                    Products = cartProducts,
                    AbonoProducts = abonoProducts,
                    ReturnedProducts = returnedProducts,
                    PaymentMethodsDropDown = methodsOrdered,
                };

                return View(editViewModel);
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
        public async Task<IActionResult> Edit(Cart cart)
        {
            try
            {
                await _workContainer.Cart.Update(cart);
                return Json(new
                {
                    success = true,
                    message = "Se ha editado la bajada correctamente",
                    data = cart.RouteID,
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error", message: "No se ha podido confirmar la bajada", error: e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Dealer)]
        public async Task<IActionResult> Confirm(Cart cart)
        {
            try
            {
                if (await _workContainer.Cart.GetFirstOrDefaultAsync(c => c.ID == cart.ID) is null)
                    return CustomBadRequest("Error", "No se ha encontrado el cliente solicitado");

                await _workContainer.Cart.Confirm(cart);
                return Json(new
                {
                    success = true,
                    title = "Confirmado",
                    message = "Se ha confirmado la bajada",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error", message: "No se ha podido confirmar la bajada", error: e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmManual(Cart cart)
        {
            try
            {
                await _workContainer.Cart.CreateManual(cart);
                return Json(new
                {
                    success = true,
                    title = "Confirmado",
                    message = "Se ha confirmado la bajada",
                    data = cart.RouteID,
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error", message: "No se ha podido confirmar la bajada", error: e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Dealer)]
        public async Task<IActionResult> SetState(long cartID, State state)
        {
            try
            {
                var cart = await _workContainer.Cart.GetOneAsync(cartID) ?? throw new Exception("No se ha encontrado la bajada solicitada");
                cart.State = state;
                await _workContainer.SaveAsync();

                return Json(new
                {
                    success = true,
                    title = "Confirmado",
                    message = "Se ha actualizado el estado de la bajada",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error", message: "No se ha podido cambiar el estado de la bajada", error: e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Dealer)]
        public async Task<IActionResult> ResetState(long cartID)
        {
            try
            {
                var cart = await _workContainer.Cart.GetOneAsync(cartID) ?? throw new Exception("No se ha encontrado la bajada solicitada");
                cart.State = State.Pending;
                await _workContainer.SaveAsync();

                return Json(new
                {
                    success = true,
                    title = "Confirmado",
                    message = "Se ha restablecido la bajada",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error", message: "No se ha podido restablecer la bajada", error: e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetReturnedProducts(long cartID)
        {
            try
            {
                var cart = await _workContainer.Cart.GetOneAsync(cartID) ?? throw new Exception("No se ha encontrado la bajada solicitada");

                var returnedProducts = await _workContainer.Cart.GetReturnedProducts(cartID);
                return Json(new
                {
                    success = true,
                    data = returnedProducts.Select(x => new
                    {
                        type = x.Type,
                        name = x.Type.GetDisplayName(),
                        quantity = x.Quantity,
                    }),
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error", message: "No se han podido recuperar los productos devueltos", error: e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Dealer)]
        public async Task<IActionResult> ReturnProducts(long cartID, List<ReturnedProduct> products)
        {
            try
            {
                await _workContainer.Cart.ReturnProducts(cartID, products);
                return Json(new
                {
                    success = true,
                    title = "Confirmado",
                    message = "Se han devuelto los productos",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error", message: "No se han podido devolver los productos", error: e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> SoftDelete(long cartID)
        {
            try
            {
                await _workContainer.Cart.SoftDelete(cartID);

                return Json(new
                {
                    success = true,
                    title = "Confirmado",
                    message = "Se ha eliminado la bajada",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error", message: "No se ha podido eliminar la bajada", error: e.Message);
            }
        }

        #endregion
    }
}
