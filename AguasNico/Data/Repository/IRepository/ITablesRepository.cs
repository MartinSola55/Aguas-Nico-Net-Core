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
        List<SoldProductsTable> GetSoldProductsByDate(DateTime date);
        List<SoldProductsTable> GetSoldProductsByRoute(long routeID);
        List<SoldProductsTable> GetSoldProductsByDateAndRoute(DateTime date, long routeID);
    }
}