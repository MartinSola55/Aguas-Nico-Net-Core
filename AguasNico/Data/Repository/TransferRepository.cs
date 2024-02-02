using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using Microsoft.EntityFrameworkCore;

namespace AguasNico.Data.Repository
{
    public class TransferRepository(ApplicationDbContext db) : Repository<Transfer>(db), ITransferRepository
    {
        private readonly ApplicationDbContext _db = db;

        public async Task Add(Transfer transfer)
        {

            var client = await _db
                .Clients
                .FirstAsync(x => x.ID == transfer.ClientID) ?? throw new Exception("No se ha encontrado el cliente");

            client.Debt -= transfer.Amount;
            if (client.DealerID != null)
                transfer.UserID = client.DealerID;

            await _db.Transfers.AddAsync(transfer);

            try
            {
                await _db.Database.BeginTransactionAsync();
                await _db.SaveChangesAsync();
                await _db.Database.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _db.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task Update(Transfer transfer)
        {

            var oldTransfer = await _db
                .Transfers
                .FirstAsync(x => x.ID == transfer.ID) ?? throw new Exception("No se ha encontrado la transferencia");

            var client = await _db
                .Clients
                .FirstAsync(x => x.ID == oldTransfer.ClientID) ?? throw new Exception("No se ha encontrado el cliente");

            client.Debt += oldTransfer.Amount;
            client.Debt -= transfer.Amount;
            if (client.DealerID != null)
            {
                oldTransfer.UserID = client.DealerID;
            }
            oldTransfer.Amount = transfer.Amount;
            oldTransfer.Date = transfer.Date;
            oldTransfer.UpdatedAt = DateTime.UtcNow.AddHours(-3);

            try
            {
                await _db.Database.BeginTransactionAsync();
                await _db.SaveChangesAsync();
                await _db.Database.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _db.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task SoftDelete(long id)
        {

                var transfer = await _db
                .Transfers
                .FirstAsync(x => x.ID == id) ?? throw new Exception("No se ha encontrado la transferencia");

                var client = await _db
                .Clients
                .FirstAsync(x => x.ID == transfer.ClientID) ?? throw new Exception("No se ha encontrado el cliente");

                client.Debt += transfer.Amount;
                transfer.DeletedAt = DateTime.UtcNow.AddHours(-3);
            try
            {
                await _db.Database.BeginTransactionAsync();
                await _db.SaveChangesAsync();
                await _db.Database.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _db.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<Transfer>> GetLastTen(long clientID)
        {
            return await _db
                .Transfers
                .Where(x => x.ClientID == clientID)
                .OrderByDescending(x => x.Date)
                .Take(10)
                .ToListAsync();
        }
    }
}