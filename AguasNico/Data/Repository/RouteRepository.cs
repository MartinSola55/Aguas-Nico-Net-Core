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
                _db.Database.BeginTransaction();

                Models.Route route = _db.Routes
                    .Where(x => x.ID == id)
                    .Include(x => x.Carts)
                    .Include(x => x.DispatchedProducts)
                    .FirstOrDefault() ?? throw new Exception("No se ha encontrado la planilla");

                foreach (DispatchedProduct product in route.DispatchedProducts)
                {
                    product.DeletedAt = DateTime.UtcNow.AddHours(-3);
                }

                CartRepository cartRepository = new(_db);
                foreach (Cart cart in route.Carts)
                {
                    cartRepository.SoftDelete(cart.ID);
                }

                route.DeletedAt = DateTime.UtcNow.AddHours(-3);
                _db.SaveChanges();
                _db.Database.CommitTransaction();
            }
            catch (Exception)
            {
                _db.Database.RollbackTransaction();
                throw;
            }
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

        public decimal GetTotalSoldByRoute(long routeID)
        {
            Models.Route route = _db.Routes.First(x => x.ID == routeID) ?? throw new Exception("No se ha encontrado la planilla");
            return _db.CartPaymentMethods
                .Where(x => x.Cart.RouteID == routeID)
                .Sum(x => x.Amount)
                + _db.Transfers
                .Where(x => x.Date.Date == route.CreatedAt.Date)
                .Sum(x => x.Amount);
        }

        public void UpdateClients(long routeID, List<Client> clients)
        {
            try
            {
                Models.Route route = _db.Routes.Where(x => x.ID == routeID).Include(x => x.Carts).First() ?? throw new Exception("No se ha encontrado la planilla");
                
                _db.Database.BeginTransaction();
                foreach (Cart cart in route.Carts)
                {
                    cart.DeletedAt = DateTime.UtcNow.AddHours(-3);
                }

                int priority = 1;
                foreach (var client in clients)
                {
                    Client dbClient = _db.Clients.First(x => x.ID == client.ID) ?? throw new Exception("No se ha encontrado uno de los clientes");
                    dbClient.UpdatedAt = DateTime.UtcNow.AddHours(-3);
                    dbClient.DealerID = route.UserID;
                    dbClient.DeliveryDay = route.DayOfWeek;
                    Cart cart = new()
                    {
                        ClientID = client.ID,
                        RouteID = routeID,
                        IsStatic = true,
                        Priority = priority++,
                        State = State.Pending,
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

        public IEnumerable<Models.Route> GetStaticsByDay(Day day)
        {
            return _db.Routes
                    .Where(x => x.IsStatic && x.DayOfWeek == day)
                    .Include(x => x.User)
                    .Include(x => x.Carts)
                    .OrderBy(x => x.User.UserName);
        }

        public IEnumerable<Models.Route> GetStaticsByDealer(string dealerID)
        {
            return _db.Routes
                    .Where(x => x.IsStatic && x.UserID == dealerID)
                    .Include(x => x.User)
                    .Include(x => x.Carts);
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

        public List<Client> ClientsInRoute(long routeID)
        {
            return
            [
                .. _db.Carts.Where(x => x.RouteID == routeID && x.IsStatic).OrderBy(x => x.Priority).Select(x => x.Client),
            ];
        }

        public List<Client> ClientsNotInRoute(long routeID)
        {
            Models.Route route = _db.Routes.First(x => x.ID == routeID) ?? throw new Exception("No se ha encontrado la planilla");
            return
            [
                .. _db.Clients.Where(x => x.IsActive && !x.Carts.Any(x => x.Route.ID == route.ID)),
            ];
        }

        public long CreateByDealer(long routeID)
        {
            try
            {
                Models.Route route = _db.Routes.Include(x => x.Carts).First(x => x.ID == routeID && x.IsStatic) ?? throw new Exception("No se ha encontrado la planilla");
            
                _db.Database.BeginTransaction();
                Models.Route newRoute = new()
                {
                    UserID = route.UserID,
                    DayOfWeek = route.DayOfWeek,
                    IsStatic = false,
                };
                _db.Routes.Add(newRoute);
                _db.SaveChanges();

                foreach (Cart cart in route.Carts)
                {
                    Cart newCart = new()
                    {
                        ClientID = cart.ClientID,
                        RouteID = newRoute.ID,
                        Priority = cart.Priority,
                        State = State.Pending,
                        IsStatic = false,
                    };
                    _db.Carts.Add(newCart);
                }
                _db.SaveChanges();
                _db.Database.CommitTransaction();
                return newRoute.ID;
            }
            catch (Exception)
            {
                _db.Database.RollbackTransaction();
                throw;
            }
        }

        public void UpdateDispatched(long routeID, List<DispatchedProduct> products)
        {
            try
            {
                Models.Route route = _db.Routes.Include(x => x.DispatchedProducts).First(x => x.ID == routeID) ?? throw new Exception("No se ha encontrado la planilla");
                _db.Database.BeginTransaction();

                foreach (DispatchedProduct dispatchedProduct in route.DispatchedProducts)
                {
                    dispatchedProduct.DeletedAt = DateTime.UtcNow.AddHours(-3);
                }

                List<DispatchedProduct> oldProducts = [.. _db.DispatchedProducts.IgnoreQueryFilters().Where(x => x.RouteID == routeID)];
                foreach (DispatchedProduct dispatchedProduct in products)
                {
                    if (oldProducts.Any(x => x.Type == dispatchedProduct.Type))
                    {
                        oldProducts.First(x => x.Type == dispatchedProduct.Type).DeletedAt = null;
                        oldProducts.First(x => x.Type == dispatchedProduct.Type).Quantity = dispatchedProduct.Quantity;
                        oldProducts.First(x => x.Type == dispatchedProduct.Type).UpdatedAt = DateTime.UtcNow.AddHours(-3);
                        _db.SaveChanges();
                    }
                    else
                    {
                        _db.DispatchedProducts.Add(new()
                        {
                            RouteID = routeID,
                            Type = dispatchedProduct.Type,
                            Quantity = dispatchedProduct.Quantity,
                        });
                    
                    }
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