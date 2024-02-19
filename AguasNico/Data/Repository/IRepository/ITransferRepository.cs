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
        Task Add(Transfer transfer);
        Task Update(Transfer transfer, bool updateDate);
        Task SoftDelete(long id);
        Task<List<Transfer>> GetLastTen(long clientID);
    }
}