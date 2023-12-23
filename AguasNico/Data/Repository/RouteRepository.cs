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
            var dbObject = _db.Routes.First(x => x.ID == transfer.ID) ?? throw new Exception("No se ha encontrado el reparto");
            dbObject.UserID = transfer.UserID;
            dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            _db.SaveChanges();
        }

        public void SoftDelete(long id)
        {
            try
            {
                var dbObject = _db.Routes.First(x => x.ID == id) ?? throw new Exception("No se ha encontrado el reparto");

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
    }
}