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

        public void Update(Abono abono)
        {
            try
            {
                Abono dbObject = _db.Abonos.First(x => x.ID == abono.ID) ?? throw new Exception("No se ha encontrado el abono");
                dbObject.Name = abono.Name;
                dbObject.Price = abono.Price;
                dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SoftDelete(long id)
        {
            try
            {
                _db.Database.BeginTransaction();

                Abono abono = _db.Abonos.Find(id) ?? throw new Exception("No se encontr� el abono");
                abono.DeletedAt = DateTime.UtcNow.AddHours(-3);

                foreach (ClientAbono clientAbono in _db.ClientAbonos.Where(x => x.AbonoID == id))
                {
                    clientAbono.DeletedAt = DateTime.UtcNow.AddHours(-3);
                }

                foreach (AbonoProduct abonoProduct in _db.AbonoProducts.Where(x => x.AbonoID == id))
                {
                    abonoProduct.DeletedAt = DateTime.UtcNow.AddHours(-3);
                }

                _db.SaveChanges();
                _db.Database.CommitTransaction();
            }
            catch (Exception)
            {
                _db.Database.RollbackTransaction();
                throw;
            }
        }

        public void RenewAll()
        {
            try
            {
                _db.Database.BeginTransaction();

                DateTime today = DateTime.UtcNow.AddHours(-3);
                foreach (ClientAbono clientAbono in _db.ClientAbonos.Include(x => x.Abono).ThenInclude(x => x.Products).Include(x => x.Client))
                {
                    if (_db.AbonoRenewals.Any(x => x.AbonoID == clientAbono.AbonoID && x.ClientID == clientAbono.ClientID && x.CreatedAt.Month == today.Month && x.CreatedAt.Year == today.Year)) continue;

                    AbonoRenewal abonoRenewal = new()
                    {
                        AbonoID = clientAbono.AbonoID,
                        ClientID = clientAbono.ClientID,
                        SettedPrice = clientAbono.Abono.Price
                    };

                    _db.AbonoRenewals.Add(abonoRenewal);
                    _db.SaveChanges();

                    foreach (AbonoProduct abonoRenewalProduct in clientAbono.Abono.Products)
                    {
                        _db.AbonoRenewalProducts.Add(new()
                        {
                            AbonoRenewalID = abonoRenewal.ID,
                            Type = abonoRenewalProduct.Type,
                            Available = abonoRenewalProduct.Quantity
                        });
                    }

                    clientAbono.Client.Debt += clientAbono.Abono.Price;
                }

                _db.SaveChanges();
                _db.Database.CommitTransaction();
            }
            catch (Exception)
            {
                _db.Database.RollbackTransaction();
                throw;
            }
        }

        public void RenewByRoute(long routeID)
        {
            try
            {
                _db.Database.BeginTransaction();

                DateTime today = DateTime.UtcNow.AddHours(-3);
                List<Client> clients = [.. _db.Routes.Where(x => x.ID == routeID).SelectMany(x => x.Carts).Select(x => x.Client)];
                foreach (ClientAbono clientAbono in _db.ClientAbonos.Where(x => clients.Contains(x.Client)).Include(x => x.Abono).ThenInclude(x => x.Products).Include(x => x.Client))
                {
                    if (_db.AbonoRenewals.Any(x => x.AbonoID == clientAbono.AbonoID && x.ClientID == clientAbono.ClientID && x.CreatedAt.Month == today.Month && x.CreatedAt.Year == today.Year)) continue;

                    AbonoRenewal abonoRenewal = new()
                    {
                        AbonoID = clientAbono.AbonoID,
                        ClientID = clientAbono.ClientID,
                        SettedPrice = clientAbono.Abono.Price
                    };

                    _db.AbonoRenewals.Add(abonoRenewal);
                    _db.SaveChanges();

                    foreach (AbonoProduct abonoRenewalProduct in clientAbono.Abono.Products)
                    {
                        _db.AbonoRenewalProducts.Add(new()
                        {
                            AbonoRenewalID = abonoRenewal.ID,
                            Type = abonoRenewalProduct.Type,
                            Available = abonoRenewalProduct.Quantity
                        });
                    }

                    clientAbono.Client.Debt += clientAbono.Abono.Price;
                }

                _db.SaveChanges();
                _db.Database.CommitTransaction();
            }
            catch (Exception)
            {
                _db.Database.RollbackTransaction();
                throw;
            }
        }

        public IEnumerable<Abono> GetLastTen(long clientID)
        {
            return _db.AbonoRenewals.Where(x => x.ClientID == clientID).OrderByDescending(x => x.CreatedAt).Take(10).Select(x => x.Abono);
        }

        public IEnumerable<Client> GetClients(long abonoID)
        {
            return _db.ClientAbonos.Where(x => x.AbonoID == abonoID && x.Client.IsActive).Include(x => x.Client).ThenInclude(x => x.Dealer).Select(x => x.Client);
        }
    }
}