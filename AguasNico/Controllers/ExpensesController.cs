﻿using AguasNico.Data.Repository.IRepository;
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
        [ActionName("Index")]
        public IActionResult Index()
        {
            try
            {
                IndexViewModel viewModel = new()
                {
                    Expenses = _workContainer.Expense.GetAll(x => x.CreatedAt.Date == DateTime.UtcNow.AddHours(-3).Date, includeProperties: "User").OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Amount),
                    Dealers = _workContainer.ApplicationUser.GetDealersDropDownList(),
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
            ModelState.Remove("CreateViewModel.User");
            if (ModelState.IsValid)
            {
                try
                {
                    Expense expense = viewModel.CreateViewModel;
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

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
        public IActionResult Edit(IndexViewModel viewModel)
        {
            ModelState.Remove("CreateViewModel.User");
            if (ModelState.IsValid)
            {
                try
                {
                    Expense expense = viewModel.CreateViewModel;
                    _workContainer.Expense.Update(expense);
                    _workContainer.Save();

                    Expense newExpense = _workContainer.Expense.GetOne(expense.ID);
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
        [ActionName("SoftDelete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Admin)]
        public IActionResult SoftDelete(long id)
        {
            try
            {
                Expense expense = _workContainer.Expense.GetOne(id) ?? throw new Exception("No se encontró el gasto");
                _workContainer.Expense.SoftDelete(id);
                _workContainer.Save();

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
        [ActionName("SearchBetweenDates")]
        [Authorize(Roles = Constants.Admin)]
        public IActionResult SearchBetweenDates(string dateFrom, string dateTo)
        {
            try
            {
                DateTime dateFromParsed = DateTime.Parse(dateFrom);
                DateTime dateToParsed = DateTime.Parse(dateTo);
                Expression<Func<Expense, bool>> filter = entity => entity.CreatedAt >= dateFromParsed && entity.CreatedAt <= dateToParsed;
                IEnumerable<Expense> expenses = _workContainer.Expense.GetAll(filter, includeProperties: "User").OrderByDescending(x => x.Amount);
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

        [HttpGet]
        [ActionName("SearchByDate")]
        [Authorize(Roles = Constants.Admin)]
        public IActionResult SearchByDate(string dateString)
        {
            try
            {
                DateTime date = DateTime.Parse(dateString);
                IEnumerable<Expense> expenses = _workContainer.Expense.GetAll(x => x.CreatedAt.Date == date.Date, includeProperties: "User").OrderByDescending(x => x.Amount);

                return Json(new
                {
                    success = true,
                    data = expenses.Select(x => new
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
