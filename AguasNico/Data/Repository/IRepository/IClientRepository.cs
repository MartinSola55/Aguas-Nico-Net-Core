using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Models;

namespace AguasNico.Data.Repository.IRepository
{
    public interface IClientRepository : IRepository<Client>
    {
        void Update(Client client);
        bool IsDuplicated(Client client);
        IEnumerable<ClientProduct> GetProducts(long clientID);
        void SoftDelete(long id);
        void AddProducInTransaction(ClientProduct clientProduct);
    }
}