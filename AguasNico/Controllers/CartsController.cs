using AguasNico.Data.Repository.IRepository;
using AguasNico.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using AguasNico.Models;
using Microsoft.AspNetCore.Identity;
using AguasNico.Models.ViewModels.Clients;

namespace AguasNico.Controllers
{
    public class CartsController(IWorkContainer workContainer, SignInManager<ApplicationUser> signInManager) : Controller
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
        [ActionName("Edit")]
        public IActionResult Edit(long id)
        {
            try
            {
                Cart cart = _workContainer.Cart.GetFirstOrDefault(x => x.ID == id, includeProperties: "Products, Products.Product, Client, ReturnedProducts, PaymentMethods, PaymentMethods.PaymentMethod") ?? throw new Exception("No se ha encontrado la bajada solicitada");
                if (cart.State != State.Confirmed) throw new Exception("No se puede editar una bajada que no esté confirmada");

                return View(cart);
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error", message: "No se ha podido recuperar la bajada", error: e.Message);
            }
        }

        [HttpPost]
        [ActionName("Confirm")]
        [ValidateAntiForgeryToken]
        public IActionResult Confirm(Cart cart)
        {
            try
            {
                if (_workContainer.Cart.GetFirstOrDefault(c => c.ID == cart.ID) is null)
                {
                    return CustomBadRequest("Error", "No se ha encontrado el cliente solicitado");
                }

                _workContainer.Cart.Confirm(cart);
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
        [ActionName("ConfirmManual")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmManual(Cart cart)
        {
            try
            {
                _workContainer.Cart.CreateManual(cart);
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
        [ActionName("SetState")]
        [ValidateAntiForgeryToken]
        public IActionResult SetState(long cartID, State state)
        {
            try
            {
                Cart cart = _workContainer.Cart.GetOne(cartID) ?? throw new Exception("No se ha encontrado la bajada solicitada");
                cart.State = state;
                _workContainer.Save();

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
        [ActionName("ResetState")]
        [ValidateAntiForgeryToken]
        public IActionResult ResetState(long cartID)
        {
            try
            {
                Cart cart = _workContainer.Cart.GetOne(cartID) ?? throw new Exception("No se ha encontrado la bajada solicitada");
                cart.State = State.Pending;
                _workContainer.Save();

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
        [ActionName("GetReturnedProducts")]
        public IActionResult GetReturnedProducts(long cartID)
        {
            try
            {
                Cart cart = _workContainer.Cart.GetOne(cartID) ?? throw new Exception("No se ha encontrado la bajada solicitada");

                return Json(new
                {
                    success = true,
                    data = _workContainer.Cart.GetReturnedProducts(cartID).Select(x => new
                    {
                        type = x.Product.Type,
                        name = x.Product.Type.GetDisplayName(),
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
        [ActionName("ReturnProducts")]
        [ValidateAntiForgeryToken]
        public IActionResult ReturnProducts(long cartID, List<ReturnedProduct> products)
        {
            try
            {
                _workContainer.Cart.ReturnProducts(cartID, products);
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
        [ActionName("SoftDelete")]
        [ValidateAntiForgeryToken]
        public IActionResult SoftDelete(long cartID)
        {
            try
            {
                _workContainer.Cart.SoftDelete(cartID);

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
    }
}
