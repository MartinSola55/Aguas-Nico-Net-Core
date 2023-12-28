using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<SelectListItem> GetYears()
        {
            var years = this.GetAll()
            .Select(route => route.CreatedAt.Year)
            .Distinct()
            .OrderByDescending(year => year)
            .ToList();

            return years.Select(year => new SelectListItem
            {
                Text = year.ToString(),
                Value = year.ToString(),
                Selected = (year == DateTime.UtcNow.AddHours(-3).Year)
            });
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

        public decimal GetTotalSold(DateTime date)
        {
            return _db.CartPaymentMethods
                .Where(x => x.CreatedAt.Date == date.Date)
                .Sum(x => x.Amount)
                + _db.Transfers
                .Where(x => x.Date.Date == date.Date)
                .Sum(x => x.Amount);
        }

        public decimal GetTotalSoldByRoute(DateTime date, long routeID)
        {
            return _db.CartPaymentMethods
                .Where(x => x.CreatedAt.Date == date.Date && x.Cart.RouteID == routeID)
                .Sum(x => x.Amount)
                + _db.Transfers
                .Where(x => x.Date.Date == date.Date)
                .Sum(x => x.Amount);
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

        public IEnumerable<Models.Route> GetStaticsByDay(Day day, string? userID = null)
        {
            return userID switch
            {
                null => _db.Routes
                    .Where(x => x.IsStatic && x.DayOfWeek == day)
                    .Include(x => x.User)
                    .Include(x => x.Carts)
                    .OrderBy(x => x.User.UserName),
                _ => _db.Routes
                    .Where(x => x.IsStatic && x.DayOfWeek == day && x.UserID == userID)
                    .Include(x => x.User)
                    .Include(x => x.Carts)
                    .OrderBy(x => x.User.UserName),
            };
        }

        public List<CartPaymentMethod> GetTotalCollected(long routeID)
        {
            List<PaymentMethod> methods = _db.PaymentMethods.ToList();
            List<CartPaymentMethod> cartPaymentMethods = new();

            foreach (PaymentMethod method in methods)
            {
                cartPaymentMethods.Add(new CartPaymentMethod()
                {
                    PaymentMethod = method,
                    Amount = _db.CartPaymentMethods
                        .Where(x => x.Cart.RouteID == routeID && x.PaymentMethodID == method.ID)
                        .Sum(x => x.Amount)
                });
            }
            return cartPaymentMethods;
        }
    }
}