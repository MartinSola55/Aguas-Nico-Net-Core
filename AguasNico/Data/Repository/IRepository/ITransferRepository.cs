using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Models;

namespace AguasNico.Data.Repository.IRepository
{
    public interface ITransferRepository : IRepository<Transfer>
    {
        new void Add(Transfer transfer);
        void Update(Transfer transfer);
        void SoftDelete(long id);
        IEnumerable<Transfer> GetLastTen(long clientID);
    }
}