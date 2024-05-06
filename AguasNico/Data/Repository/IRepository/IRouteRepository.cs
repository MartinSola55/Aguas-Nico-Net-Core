using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Models;
using AguasNico.Models.ViewModels.Routes.Details;

namespace AguasNico.Data.Repository.IRepository
{
    public interface IRouteRepository : IRepository<Models.Route>
    {
        Task<List<SelectListItem>> GetYears();
        Task Update(Models.Route route);
        Task SoftDelete(long id);
        Task Close(long id);
        Task<decimal> GetTotalSold(DateTime date);
        Task<decimal> GetTotalSoldByRoute(long routeID);
        Task UpdateClients(long routeID, List<Client> clients);
        Task UpdateDispatched(long routeID, List<DispatchedProduct> products);
        Task<List<Models.Route>> GetStaticsByDay(Day day);
        Task<List<Models.Route>> GetStaticsByDealer(string dealerID);
        Task<List<CartPaymentMethod>> GetTotalCollected(long routeID);
        Task<List<Client>> ClientsInRoute(long routeID);
        Task<List<Client>> ClientsNotInRoute(long routeID);
        Task<List<Client>> ClientsByNameNotInRoute(long routeID, string name);
        Task<Client?> ClientsByIDNotInRoute(long routeID, long clientID);
        Task<long> CreateByDealer(long routeID);
        Task SetDispenserPrice(long routeID, decimal price);
    }
}