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
        void Update(Models.Route route);
        void SoftDelete(long id);
        decimal GetTotalSold(DateTime date);
        void UpdateClients(long routeID, List<Client> clients);
        IEnumerable<Models.Route> GetStaticsByDay(Day day, string? userID = null);
    }
}