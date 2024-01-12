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
        void Update(Abono abono);
        void SoftDelete(long id);
        void RenewAll();
        void RenewByRoute(long routeID);
    }
}