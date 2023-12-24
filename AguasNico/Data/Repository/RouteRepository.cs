using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using Microsoft.AspNetCore.Mvc;

namespace AguasNico.Data.Repository
{
    public class RouteRepository(ApplicationDbContext db) : Repository<Models.Route>(db), IRouteRepository
    {
        private readonly ApplicationDbContext _db = db;

        public void Update(Models.Route transfer)
        {
            var dbObject = _db.Routes.First(x => x.ID == transfer.ID) ?? throw new Exception("No se ha encontrado la planilla");
            dbObject.UserID = transfer.UserID;
            dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            _db.SaveChanges();
        }

        public void SoftDelete(long id)
        {
            try
            {
                var dbObject = _db.Routes.First(x => x.ID == id) ?? throw new Exception("No se ha encontrado la planilla");

            }
            catch (Exception)
            {
                _db.Database.RollbackTransaction();
                throw;
            }
        }

        public ApplicationDbContext Get_db()
        {
            return _db;
        }

        public void GetTotalSold(DateTime date)
        {
            var asd = _db.CartPaymentMethods
                .Where(x => x.CreatedAt.Date == date.Date)
                .GroupBy(x => x.PaymentMethod)
                .Select(g => new {
                    PaymentMethodID = g.Key,
                    TotalAmount = g.Sum(x => x.Amount)
                });
        }

        public void UpdateClients(long routeID, List<Client> clients)
        {
            try
            {
                var dbObject = _db.Routes.First(x => x.ID == routeID) ?? throw new Exception("No se ha encontrado la planilla");
                
                _db.Database.BeginTransaction();
                foreach (Cart cart in dbObject.Carts)
                {
                    cart.DeletedAt = DateTime.UtcNow.AddHours(-3);
                }

                int priority = 1;
                foreach (var client in clients)
                {
                    Cart cart = new()
                    {
                        ClientID = client.ID,
                        RouteID = routeID,
                        CreatedAt = DateTime.UtcNow.AddHours(-3),
                        IsStatic = true,
                        Priority = priority++,
                    };
                    _db.Carts.Add(cart);
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
    }
}