using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using AguasNico.Models.ViewModels.Routes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace AguasNico.Controllers
{
    [Authorize]
    public class RouteController(IWorkContainer workContainer, SignInManager<ApplicationUser> signInManager) : Controller
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
                Expression<Func<Models.Route, bool>> filter;
                switch (role)
                {
                    case Constants.Admin:
                        filter = entity => entity.DayOfWeek == (Day)DateTime.UtcNow.AddHours(-3).DayOfWeek && entity.IsStatic;
                        viewModel.Routes = _workContainer.Route.GetAll(filter, includeProperties: "User.UserName, Carts").OrderBy(x => x.User.UserName);
                        break;
                    case Constants.Dealer:
                        filter = entity => entity.UserID == user.Id && entity.IsStatic;
                        viewModel.Routes = _workContainer.Route.GetAll(filter, includeProperties: "User.UserName, Carts").OrderBy(x => x.User.UserName);
                        break;
                    default:
                        return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
                }
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
                Models.Route route = _workContainer.Route.GetFirstOrDefault(x => x.ID == id, includeProperties: "User.UserName, Carts, Carts.Client, Carts.CartPaymentMethod, Carts.CartPaymentMethod.PaymentMethod");
                if (route is null)
                {
                    return CustomBadRequest(title: "Error al obtener la planilla", message: "La planilla no existe");
                }
                
                DetailsViewModel viewModel = new()
                {
                    Route = route,
                    PaymentMethods = _workContainer.PaymentMethod.GetDropDownList(),
                };

                if (role == Constants.Admin)
                {
                    viewModel.DispatchedProducts = _workContainer.DispatchedProduct.GetAllFromRoute(id).OrderBy(x => x.Product.Name);
                    // TODO: Get stats
                }
                return View(viewModel);
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al obtener la planilla", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
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
    }
}
