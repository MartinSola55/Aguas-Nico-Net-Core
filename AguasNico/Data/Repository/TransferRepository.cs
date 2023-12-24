using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;

namespace AguasNico.Data.Repository
{
    public class TransferRepository(ApplicationDbContext db) : Repository<Transfer>(db), ITransferRepository
    {
        private readonly ApplicationDbContext _db = db;

        public new void Add(Transfer transfer)
        {
            try
            {
                Client client = _db.Clients.First(x => x.ID == transfer.ClientID) ?? throw new Exception("No se ha encontrado el cliente");
                _db.Database.BeginTransaction();
                client.Debt -= transfer.Amount;
                _db.Transfers.Add(transfer);
                _db.SaveChanges();
                _db.Database.CommitTransaction();
            }
            catch (Exception)
            {
                _db.Database.RollbackTransaction();
                throw;
            }
        }

        public void Update(Transfer transfer)
        {
            var dbObject = _db.Transfers.First(x => x.ID == transfer.ID) ?? throw new Exception("No se ha encontrado la transferencia");
            dbObject.UserID = transfer.UserID;
            dbObject.ClientID = transfer.ClientID;
            dbObject.Amount = transfer.Amount;
            dbObject.Date = transfer.Date;
            dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            _db.SaveChanges();
        }

        public void SoftDelete(long id)
        {
            try
            {
                var dbObject = _db.Transfers.First(x => x.ID == id) ?? throw new Exception("No se ha encontrado la transferencia");
                Client client = _db.Clients.First(x => x.ID == dbObject.ClientID) ?? throw new Exception("No se ha encontrado el cliente");
                _db.Database.BeginTransaction();
                client.Debt += dbObject.Amount;
                dbObject.DeletedAt = DateTime.UtcNow.AddHours(-3);
                _db.SaveChanges();
                _db.Database.CommitTransaction();
            }
            catch (Exception)
            {
                _db.Database.RollbackTransaction();
                throw;
            }
        }

        public IEnumerable<Transfer> GetLastTen(long clientID)
        {
            return _db.Transfers.Where(x => x.ClientID == clientID).OrderByDescending(x => x.Date).Take(10);
        }
    }
}