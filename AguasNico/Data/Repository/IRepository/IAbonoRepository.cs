using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Models;

namespace AguasNico.Data.Repository.IRepository
{
    public interface IAbonoRepository : IRepository<Abono>
    {
        Task Update(Abono abono);
        Task SoftDelete(long id);
        Task RenewAll();
        Task RenewByRoute(long routeID);
        Task<List<AbonoRenewal>> GetLastTen(long clientID);
        Task<List<Client>> GetClients(long abonoID);
    }
}