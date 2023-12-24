using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;

namespace AguasNico.Data.Repository
{
    public class ClientRepository(ApplicationDbContext db) : Repository<Client>(db), IClientRepository
    {
        private readonly ApplicationDbContext _db = db;

        public void Update(Client client)
        {
            var dbObject = _db.Clients.First(x => x.ID == client.ID) ?? throw new Exception("No se ha encontrado el cliente");
            dbObject.Name = client.Name;
            dbObject.Address = client.Address;
            dbObject.Phone = client.Phone;
            dbObject.Observations = client.Observations;
            dbObject.Debt = client.Debt;
            dbObject.HasInvoice = client.HasInvoice;
            dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            _db.SaveChanges();
        }

        public IEnumerable<ClientProduct> GetProducts(long clientID)
        {
            return _db.ClientProducts.Where(x => x.ClientID == clientID);
        }

        public bool IsDuplicated(Client client)
        {
            return _db.Clients.Any(x => x.Name == client.Name && x.Address == client.Address);
        }

        public void SoftDelete(long id)
        {
            var dbObject = _db.Clients.First(x => x.ID == id) ?? throw new Exception("No se ha encontrado el cliente");
            dbObject.DeletedAt = DateTime.UtcNow.AddHours(-3);
            _db.SaveChanges();
        }

        public void AddProducInTransaction(ClientProduct clientProduct)
        {
            _db.ClientProducts.Add(clientProduct);
        }
    }
}