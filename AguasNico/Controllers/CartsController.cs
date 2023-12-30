using AguasNico.Data.Repository.IRepository;
using AguasNico.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using AguasNico.Models;
using Microsoft.AspNetCore.Identity;

namespace AguasNico.Controllers
{
    public class CartsController(IWorkContainer workContainer, SignInManager<ApplicationUser> signInManager) : Controller
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
    }
}
