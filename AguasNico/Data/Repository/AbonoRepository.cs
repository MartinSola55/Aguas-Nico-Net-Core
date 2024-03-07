using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using System.Security.Cryptography.Xml;
using Microsoft.EntityFrameworkCore;

namespace AguasNico.Data.Repository
{
    public class AbonoRepository(ApplicationDbContext db) : Repository<Abono>(db), IAbonoRepository
    {
        private readonly ApplicationDbContext _db = db;

        public async Task Update(Abono abono)
        {
            var dbObject = await _db
                .Abonos
                .FirstAsync(x => x.ID == abono.ID) ?? throw new Exception("No se encontró el abono");

            dbObject.Name = abono.Name;
            dbObject.Price = abono.Price;
            dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);

            await _db.SaveChangesAsync();
        }

        public async Task SoftDelete(long id)
        {
            var abono = await _db
                .Abonos
                .FindAsync(id) ?? throw new Exception("No se encontró el abono");

            abono.DeletedAt = DateTime.UtcNow.AddHours(-3);

            foreach (var clientAbono in await _db.ClientAbonos.Where(x => x.AbonoID == id).ToListAsync())
            {
                clientAbono.DeletedAt = DateTime.UtcNow.AddHours(-3);
            }

            foreach (var abonoProduct in await _db.AbonoProducts.Where(x => x.AbonoID == id).ToListAsync())
            {
                abonoProduct.DeletedAt = DateTime.UtcNow.AddHours(-3);
            }
            
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

        public async Task RenewAll()
        {
            DateTime today = DateTime.UtcNow.AddHours(-3);
            var clientAbonos = await _db
                .ClientAbonos
                .Include(x => x.Abono)
                    .ThenInclude(x => x.Products)
                .Include(x => x.Client)
                .ToListAsync();

            foreach (var clientAbono in clientAbonos)
            {
                var abonoRenewed = await _db
                    .AbonoRenewals
                    .AnyAsync(x => x.AbonoID == clientAbono.AbonoID && x.ClientID == clientAbono.ClientID && x.CreatedAt.Month == today.Month && x.CreatedAt.Year == today.Year);
                if (abonoRenewed) continue;

                var abonoRenewal = new AbonoRenewal()
                {
                    AbonoID = clientAbono.AbonoID,
                    ClientID = clientAbono.ClientID,
                    SettedPrice = clientAbono.Abono.Price
                };

                await _db.AbonoRenewals.AddAsync(abonoRenewal);

                foreach (AbonoProduct abonoRenewalProduct in clientAbono.Abono.Products)
                {
                    await _db.AbonoRenewalProducts.AddAsync(new()
                    {
                        AbonoRenewal = abonoRenewal,
                        Type = abonoRenewalProduct.Type,
                        Available = abonoRenewalProduct.Quantity
                    });
                }

                clientAbono.Client.Debt += clientAbono.Abono.Price;
            }
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

        public async Task RenewByRoute(long routeID)
        {
            DateTime today = DateTime.UtcNow.AddHours(-3);
            var clients = await _db
                .Routes
                .Where(x => x.ID == routeID)
                .SelectMany(x => x.Carts)
                .Select(x => x.Client)
                .AsNoTracking()
                .ToListAsync();

            var clientAbonos = await _db
                .ClientAbonos
                .Where(x => clients.Contains(x.Client))
                .Include(x => x.Abono)
                    .ThenInclude(x => x.Products)
                .Include(x => x.Client)
                .ToListAsync();

            foreach (var clientAbono in clientAbonos)
            {
                var abonoRenewed = await _db
                    .AbonoRenewals
                    .AnyAsync(x => x.AbonoID == clientAbono.AbonoID && x.ClientID == clientAbono.ClientID && x.CreatedAt.Month == today.Month && x.CreatedAt.Year == today.Year);
                if (abonoRenewed) continue;

                var abonoRenewal = new AbonoRenewal()
                {
                    AbonoID = clientAbono.Abono.ID,
                    ClientID = clientAbono.Client.ID,
                    SettedPrice = clientAbono.Abono.Price,
                    ProductsAvailables = []
                };

                await _db.AbonoRenewals.AddAsync(abonoRenewal);

                foreach (var abonoRenewalProduct in clientAbono.Abono.Products)
                {
                    abonoRenewal.ProductsAvailables.Add(new()
                    {
                        Type = abonoRenewalProduct.Type,
                        Available = abonoRenewalProduct.Quantity
                    });
                }

                clientAbono.Client.Debt += clientAbono.Abono.Price;
            }
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

        public async Task<List<AbonoRenewal>> GetLastTen(long clientID)
        {
            return await _db
                .AbonoRenewals
                .Where(x => x.ClientID == clientID)
                .OrderByDescending(x => x.CreatedAt)
                .Take(10)
                .Include(x => x.Abono)
                .ToListAsync();
        }

        public async Task<List<Client>> GetClients(long abonoID)
        {
            return await _db
                .ClientAbonos
                .Where(x => x.AbonoID == abonoID && x.Client.IsActive)
                .Include(x => x.Client)
                .ThenInclude(x => x.Dealer)
                .Select(x => x.Client)
                .ToListAsync();
        }
    }
}