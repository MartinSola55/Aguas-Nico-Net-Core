using AguasNico.Data.Repository.IRepository;
using AguasNico.Models.ViewModels.Products;
using AguasNico.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AguasNico.Controllers
{
    [Authorize(Roles = Constants.Admin)]
    public class ProductsController(IWorkContainer workContainer, IConfiguration configuration) : BaseController(configuration)
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
        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await _workContainer.Product.GetAllAsync(x => x.IsActive);
                IndexViewModel viewModel = new()
                {
                    Products = [.. products.OrderBy(x => x.SortOrder).ThenBy(x => x.Name).ThenBy(x => x.Price) ],
                    Product = new Product(),
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
        public async Task<IActionResult> Create(Product product)
        {
            ModelState.Remove("product.ID");
            if (ModelState.IsValid)
            {
                try
                {
                    if (product.Type == 0)
                        return CustomBadRequest(title: "Error al crear el producto", message: "Debe seleccionar un tipo de producto");

                    if (await _workContainer.Product.IsDuplicated(product))
                        return CustomBadRequest(title: "Error al crear el producto", message: "Ya existe uno con el mismo nombre y precio");

                    await _workContainer.Product.AddAsync(product);
                    await _workContainer.SaveAsync();

                    var newProduct = await _workContainer.Product.GetFirstOrDefaultAsync(p => product.ID.Equals(p.ID));
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (await _workContainer.Product.IsDuplicated(product))
                        return CustomBadRequest(title: "Error al editar el producto", message: "Ya existe uno con el mismo nombre y precio");
                    
                    await _workContainer.Product.Update(product);
                    
                    var editedProduct = _workContainer.Product.GetFirstOrDefaultAsync(p => product.ID.Equals(p.ID));
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoftDelete(long id)
        {
            try
            {
                var product = await _workContainer.Product.GetOneAsync(id);

                if (product == null)
                    return CustomBadRequest(title: "Error al eliminar", message: "No se encontró el producto solicitado");

                await _workContainer.Product.SoftDelete(id);
                
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

        #endregion

        #region AJAX

        [HttpGet]
        public async Task<IActionResult> GetClients(long productID)
        {
            try
            {
                return Json(new
                {
                    success = true,
                    data = await _workContainer.Product.GetClients(productID),
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al obtener los clientes", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        #endregion
    }
}
