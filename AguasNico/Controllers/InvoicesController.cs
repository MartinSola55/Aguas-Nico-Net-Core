using AguasNico.Data.Repository.IRepository;
using AguasNico.Models.ViewModels.Invoices;
using AguasNico.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Models.ViewModels.Tables;

namespace AguasNico.Controllers
{
    [Authorize(Roles = Constants.Admin)]
    public class InvoicesController(IWorkContainer workContainer) : Controller
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
                List<SelectListItem> days =
                [
                    new() { Value = "", Text = "Seleccione un día", Disabled = true, Selected = true }
                ];
                foreach (Day day in Enum.GetValues(typeof(Day)))
                {
                    days.Add(new SelectListItem { Value = ((int)day).ToString(), Text = day.ToString() });
                }
                IndexViewModel viewModel = new()
                {
                    Days = days,
                    Dealers = _workContainer.ApplicationUser.GetDealersDropDownList()
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        [HttpGet]
        public IActionResult Show(string dateRange, Day invoiceDay, string invoiceDealer)
        {
            try
            {
                DateTime startDate = DateTime.Parse(dateRange.Split('-')[0].Trim());
                DateTime endDate = DateTime.Parse(dateRange.Split('-')[1].Trim());

                List<InvoiceTable> clients = _workContainer.Tables.GetInvoicesByDates(startDate, endDate, invoiceDay, invoiceDealer);

                return Json(new
                {
                    success = true,
                    data = clients
                });
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }
    }
}
