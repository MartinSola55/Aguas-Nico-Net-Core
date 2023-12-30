using AguasNico.Data.Repository.IRepository;
using AguasNico.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using AguasNico.Models;
using Microsoft.AspNetCore.Identity;

namespace AguasNico.Controllers
{
    public class CartController(IWorkContainer workContainer, SignInManager<ApplicationUser> signInManager) : Controller
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
    }
}
