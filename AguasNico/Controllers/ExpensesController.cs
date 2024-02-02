using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using AguasNico.Models.ViewModels.Expenses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace AguasNico.Controllers
{
    [Authorize]
    public class ExpensesController(IWorkContainer workContainer) : Controller
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
        public async Task<IActionResult> Index()
        {
            try
            {
                var expenses = await _workContainer.Expense.GetAllAsync(x => x.CreatedAt.Date == DateTime.UtcNow.AddHours(-3).Date, includeProperties: "User");
                IndexViewModel viewModel = new()
                {
                    Expenses = expenses.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Amount),
                    Dealers = await _workContainer.ApplicationUser.GetDealersDropDownList(),
                    CreateViewModel = new Expense()
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IndexViewModel viewModel)
        {
            ModelState.Remove("CreateViewModel.User");
            if (ModelState.IsValid)
            {
                try
                {
                    var expense = viewModel.CreateViewModel;
                    await _workContainer.Expense.AddAsync(expense);
                    await _workContainer.SaveAsync();

                    var newExpense = await _workContainer.Expense.GetOneAsync(expense.ID);
                    return Json(new
                    {
                        success = true,
                        data = newExpense,
                        message = "El gasto se creó correctamente",
                    });
                }
                catch (Exception e)
                {
                    return CustomBadRequest(title: "Error al crear el gasto", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
                }
            }
            return CustomBadRequest(title: "Error al crear el gasto", message: "Alguno de los campos ingresados no es válido");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> Edit(IndexViewModel viewModel)
        {
            ModelState.Remove("CreateViewModel.User");
            if (ModelState.IsValid)
            {
                try
                {
                    var expense = viewModel.CreateViewModel;
                    await _workContainer.Expense.Update(expense);

                    var newExpense = await _workContainer.Expense.GetOneAsync(expense.ID);
                    return Json(new
                    {
                        success = true,
                        data = newExpense,
                        message = "El gasto se editó correctamente",
                    });
                }
                catch (Exception e)
                {
                    return CustomBadRequest(title: "Error al editar el gasto", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
                }
            }
            return CustomBadRequest(title: "Error al editar el gasto", message: "Alguno de los campos ingresados no es válido");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> SoftDelete(long id)
        {
            try
            {
                var expense = await _workContainer.Expense.GetOneAsync(id) ?? throw new Exception("No se encontró el gasto");
                await _workContainer.Expense.SoftDelete(id);

                return Json(new
                {
                    success = true,
                    data = id,
                    message = "El gasto se eliminó correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al eliminar el gasto", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> SearchBetweenDates(string dateFrom, string dateTo)
        {
            try
            {
                var dateFromParsed = DateTime.Parse(dateFrom);
                var dateToParsed = DateTime.Parse(dateTo);

                var expenses = await _workContainer.Expense.GetAllAsync(x => x.CreatedAt >= dateFromParsed && x.CreatedAt <= dateToParsed, includeProperties: "User");
                return Json(new
                {
                    success = true,
                    data = expenses.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Amount),
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al buscar el gasto", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = Constants.Admin)]
        public async Task<IActionResult> SearchByDate(string dateString)
        {
            try
            {
                var date = DateTime.Parse(dateString);
                var expenses = await _workContainer.Expense.GetAllAsync(x => x.CreatedAt.Date == date.Date, includeProperties: "User");

                return Json(new
                {
                    success = true,
                    data = expenses.OrderByDescending(x => x.Amount).Select(x => new
                    {
                        dealer = x.User.UserName,
                        description = x.Description,
                        amount = x.Amount
                    })
                });
            }
            catch (Exception)
            {
                return CustomBadRequest(title: "No se encontraron planillas", message: "Intente nuevamente o comuníquese para soporte");
            }
        }
    }
}
