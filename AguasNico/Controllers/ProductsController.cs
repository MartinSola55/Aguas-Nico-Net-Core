using AguasNico.Data.Repository.IRepository;
using AguasNico.Models.ViewModels.Products;
using AguasNico.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AguasNico.Controllers
{
    [Authorize]
    public class ProductsController(IWorkContainer workContainer) : Controller
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
        public IActionResult Index()
        {
            try
            {
                IndexViewModel viewModel = new()
                {
                    Products = _workContainer.Product.GetAll().OrderBy(x => x.Name).ThenBy(x => x.Price),
                    CreateViewModel = new Product(),
                    ProductTypes = _workContainer.Product.GetTypes()
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
        public IActionResult Create(IndexViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Product product = viewModel.CreateViewModel;
                    if (_workContainer.Product.IsDuplicated(product))
                    {
                        return BadRequest(new
                        {
                            success = false,
                            title = "Error al crear el producto",
                            message = "Ya existe uno con el mismo nombre y precio",
                        });
                    }
                    product.CreatedAt = DateTime.UtcNow.AddHours(-3);
                    _workContainer.Product.Add(product);
                    _workContainer.Save();

                    Product newProduct = _workContainer.Product.GetFirstOrDefault(p => product.ID.Equals(p.ID));
                    return Json(new
                    {
                        success = true,
                        data = newProduct,
                        message = "El producto se creó correctamente",
                    });
                }
                catch (Exception e)
                {
                    return CustomBadRequest(title: "Error al crear el producto", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
                }
            }
            return CustomBadRequest(title: "Error al crear el producto", message: "Alguno de los campos ingresados no es válido");
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(IndexViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Product product = viewModel.CreateViewModel;
                    if (_workContainer.Product.IsDuplicated(product))
                    {
                        return BadRequest(new
                        {
                            success = false,
                            title = "Error al crear el producto",
                            message = "Ya existe uno con el mismo nombre y precio",
                        });
                    }
                    _workContainer.Product.Update(product);
                    _workContainer.Save();
                    Product editedProduct = _workContainer.Product.GetFirstOrDefault(p => product.ID.Equals(p.ID));
                    return Json(new
                    {
                        success = true,
                        data = editedProduct,
                        message = "El producto se editó correctamente",
                    });
                }
                catch (Exception e)
                {
                    return CustomBadRequest(title: "Error al editar el producto", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
                }
            }
            return CustomBadRequest(title: "Error al editar el producto", message: "Alguno de los campos ingresados no es válido");

        }

        [HttpPost]
        [ActionName("SoftDelete")]
        [ValidateAntiForgeryToken]
        public IActionResult SoftDelete(long id)
        {
            try
            {
                Product product = _workContainer.Product.GetOne(id);
                if (product == null)
                {
                    return CustomBadRequest(title: "Error al eliminar", message: "No se encontró el producto solicitado");
                }

                _workContainer.Product.SoftDelete(id);
                _workContainer.Save();
                return Json(new
                {
                    success = true,
                    data = id,
                    message = "El producto se eliminó correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al eliminar el producto", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpGet]
        [ActionName("GetClients")]
        public IActionResult GetClients(long productID)
        {
            try
            {
                return Json(new
                {
                    success = true,
                    data = _workContainer.Product.GetClients(productID).ToList(),
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al obtener los clientes", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }
    }
}
