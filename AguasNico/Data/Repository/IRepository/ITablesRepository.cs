using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Models;
using AguasNico.Models.ViewModels.Tables;

namespace AguasNico.Data.Repository.IRepository
{
    public interface ITablesRepository
    {
        Task<List<SoldProductsTable>> GetSoldProductsByDate(DateTime date);
        Task<List<SoldProductsTable>> GetSoldProductsByRoute(long routeID);
        Task<List<SoldProductsTable>> GetSoldProductsByDateAndRoute(DateTime date, long routeID);
        Task<List<SoldProductsTable>> GetSoldProductsBetweenDates(DateTime dateFrom, DateTime dateTo, string dealerID);
        Task<List<InvoiceTable>> GetInvoicesByDates(DateTime startDate, DateTime endDate, Day invoiceDay, string invoiceDealer);
    }
}