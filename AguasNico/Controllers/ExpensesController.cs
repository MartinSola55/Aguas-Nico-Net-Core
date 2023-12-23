using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using AguasNico.Models.ViewModels.Expenses;
using Microsoft.AspNetCore.Authorization;
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
        public IActionResult Index()
        {
            try
            {
                Expression<Func<Expense, bool>> filter = entity => entity.CreatedAt.Date == DateTime.UtcNow.AddHours(-3).Date;
                IndexViewModel viewModel = new()
                {
                    Expenses = _workContainer.Expense.GetAll(filter, includeProperties: "User.UserName").OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Amount),
                    Dealers = _workContainer.ApplicationUser.GetDropDownList(),
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
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IndexViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Expense expense = viewModel.CreateViewModel;

                    expense.CreatedAt = DateTime.UtcNow.AddHours(-3);
                    _workContainer.Expense.Add(expense);
                    _workContainer.Save();

                    Expense newExpense = _workContainer.Expense.GetOne(expense.ID);
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

        [HttpGet]
        public IActionResult SearchByDates(string dateFrom, string dateTo)
        {
            try
            {
                DateTime dateFromParsed = DateTime.Parse(dateFrom);
                DateTime dateToParsed = DateTime.Parse(dateTo);
                Expression<Func<Expense, bool>> filter = entity => entity.CreatedAt >= dateFromParsed && entity.CreatedAt <= dateToParsed;
                IEnumerable<Expense> expenses = _workContainer.Expense.GetAll(filter, includeProperties: "User.UserName").OrderByDescending(x => x.Amount);
                return Json(new
                {
                    success = true,
                    data = expenses,
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al buscar el gasto", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }
    }
}
