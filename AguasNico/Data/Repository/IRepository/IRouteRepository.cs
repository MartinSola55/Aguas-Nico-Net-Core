using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Models;

namespace AguasNico.Data.Repository.IRepository
{
    public interface IRouteRepository : IRepository<Models.Route>
    {
        IEnumerable<SelectListItem> GetYears();
        void Update(Models.Route route);
        void SoftDelete(long id);
        decimal GetTotalSold(DateTime date);
        decimal GetTotalSoldByRoute(DateTime date, long routeID);
        void UpdateClients(long routeID, List<Client> clients);
        IEnumerable<Models.Route> GetStaticsByDay(Day day, string? userID = null);
        List<CartPaymentMethod> GetTotalCollected(long routeID);
    }
}