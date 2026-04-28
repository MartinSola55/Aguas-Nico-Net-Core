using AguasNico.Data.Repository.IRepository;
using AguasNico.Models.ViewModels.Invoices;
using AguasNico.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace AguasNico.Controllers
{
    [Authorize(Roles = Constants.Admin)]
    public class InvoicesController(IWorkContainer workContainer, IConfiguration configuration) : BaseController(configuration)
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
                IndexViewModel viewModel = new()
                {
                    Dealers = await _workContainer.ApplicationUser.GetDealersDropDownList()
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        #endregion

        #region AJAX

        [HttpGet]
        public async Task<IActionResult> Show(string dateRange, Day? invoiceDay, string invoiceDealer)
        {
            try
            {
                var startDate = DateTime.Parse(dateRange.Split('-')[0].Trim());
                var endDate = DateTime.Parse(dateRange.Split('-')[1].Trim());

                var clients = await _workContainer.Tables.GetInvoicesByDates(startDate, endDate, invoiceDay, invoiceDealer);

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

        [HttpGet]
        public async Task<IActionResult> DownloadCsv(string dateRange, Day? invoiceDay, string invoiceDealer)
        {
            try
            {
                var startDate = DateTime.Parse(dateRange.Split('-')[0].Trim());
                var endDate = DateTime.Parse(dateRange.Split('-')[1].Trim());

                var rows = await _workContainer.Tables.GetInvoicesCsvData(startDate, endDate, invoiceDay, invoiceDealer);

                var sb = new StringBuilder();
                sb.AppendLine("external_id,cuit_cliente,punto_venta,tipo_comprobante,neto,iva_alicuota,total,tax_condition_type_id,client_name,client_address,description,email");

                foreach (var row in rows)
                {
                    sb.AppendLine(string.Join(",", [
                        EscapeCsvField(row.ExternalId),
                        EscapeCsvField(row.ClientCuit),
                        Constants.INVOICE_SALES_POINT.ToString(),
                        EscapeCsvField(row.InvoiceTypeId),
                        ((int)row.Neto).ToString(),
                        row.IvaRate.ToString(),
                        ((int)row.Total).ToString(),
                        row.TaxConditionTypeId.ToString(),
                        EscapeCsvField(row.ClientName),
                        EscapeCsvField(row.ClientAddress),
                        EscapeCsvField(row.Description),
                        EscapeCsvField(row.Email),
                    ]));
                }

                var bytes = Encoding.UTF8.GetBytes(sb.ToString());
                return File(bytes, "text/csv", $"facturas_{startDate:dd-MM-yyy}_{endDate:dd-MM-yyy}.csv");
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        private static string EscapeCsvField(string value)
        {
            if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
                return $"\"{value.Replace("\"", "\"\"")}\"";
            return value;
        }

        #endregion
    }
}
