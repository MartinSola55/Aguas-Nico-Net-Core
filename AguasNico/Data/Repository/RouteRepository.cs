using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AguasNico.Models.ViewModels.Routes.Details;

namespace AguasNico.Data.Repository
{
    public class RouteRepository(ApplicationDbContext db) : Repository<Models.Route>(db), IRouteRepository
    {
        private readonly ApplicationDbContext _db = db;

        public async Task Update(Models.Route route)
        {
            var oldRoute = await _db
                .Routes
                .FirstAsync(x => x.ID == route.ID) ?? throw new Exception("No se ha encontrado la planilla");

            oldRoute.UserID = route.UserID;
            oldRoute.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            await _db.SaveChangesAsync();
        }

        public async Task<List<SelectListItem>> GetYears()
        {
            var years = await _db
                .Routes
                .Select(route => route.CreatedAt.Year)
                .Distinct()
                .OrderByDescending(year => year)
                .ToListAsync();

            return years.Select(year => new SelectListItem
            {
                Text = year.ToString(),
                Value = year.ToString(),
                Selected = (year == DateTime.UtcNow.AddHours(-3).Year)
            }).ToList();
        }

        public async Task SoftDelete(long id)
        {
            var route = await _db
                .Routes
                .Where(x => x.ID == id)
                .Include(x => x.Carts)
                .Include(x => x.DispatchedProducts)
                .FirstOrDefaultAsync() ?? throw new Exception("No se ha encontrado la planilla");

            foreach (var product in route.DispatchedProducts)
            {
                product.DeletedAt = DateTime.UtcNow.AddHours(-3);
            }

            try
            {
                await _db.Database.BeginTransactionAsync();

                CartRepository cartRepository = new(_db);
                foreach (var cart in route.Carts)
                {
                    await cartRepository.SoftDelete(cart.ID);
                }

                route.DeletedAt = DateTime.UtcNow.AddHours(-3);

                await _db.SaveChangesAsync();
                await _db.Database.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _db.Database.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task<decimal> GetTotalSold(DateTime date)
        {
            return await _db
                .CartPaymentMethods
                .Where(x => x.CreatedAt.Date == date.Date)
                .SumAsync(x => x.Amount)
                +
                await _db
                .Transfers
                .Where(x => x.Date.Date == date.Date)
                .SumAsync(x => x.Amount);
        }

        public async Task<decimal> GetTotalSoldByRoute(long routeID)
        {
           var route = await _db
                .Routes
                .FirstAsync(x => x.ID == routeID) ?? throw new Exception("No se ha encontrado la planilla");

            return await _db
                .CartPaymentMethods
                .Where(x => x.Cart.RouteID == routeID)
                .SumAsync(x => x.Amount)
                + 
                await _db
                .Transfers
                .Where(x => x.Date.Date == route.CreatedAt.Date)
                .SumAsync(x => x.Amount);
        }

        public async Task UpdateClients(long routeID, List<Client> clients)
        {
            var route = await _db
                .Routes
                .Where(x => x.ID == routeID)
                .Include(x => x.Carts)
                .FirstAsync() ?? throw new Exception("No se ha encontrado la planilla");
                
            foreach (var cart in route.Carts)
            {
                cart.DeletedAt = DateTime.UtcNow.AddHours(-3);
            }

            int priority = 1;
            foreach (var client in clients)
            {
                var dbClient = await _db
                    .Clients
                    .FirstAsync(x => x.ID == client.ID) ?? throw new Exception("No se ha encontrado uno de los clientes");

                dbClient.UpdatedAt = DateTime.UtcNow.AddHours(-3);
                dbClient.DealerID = route.UserID;
                dbClient.DeliveryDay = route.DayOfWeek;

                var cart = new Cart()
                {
                    ClientID = client.ID,
                    RouteID = routeID,
                    IsStatic = true,
                    Priority = priority++,
                    State = State.Pending,
                };
                await _db.Carts.AddAsync(cart);
            }
                
            route.UpdatedAt = DateTime.UtcNow.AddHours(-3);

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

        public async Task<List<Models.Route>> GetStaticsByDay(Day day)
        {
            return await _db
                .Routes
                .Where(x => x.IsStatic && x.DayOfWeek == day)
                .Include(x => x.User)
                .Include(x => x.Carts)
                .OrderBy(x => x.User.UserName)
                .ToListAsync();
        }

        public async Task<List<Models.Route>> GetStaticsByDealer(string dealerID)
        {
            return await _db
                .Routes
                .Where(x => x.IsStatic && x.UserID == dealerID)
                .Include(x => x.User)
                .Include(x => x.Carts)
                .ToListAsync();
        }

        public async Task<List<CartPaymentMethod>> GetTotalCollected(long routeID)
        {
            var methods = await _db
                .PaymentMethods
                .ToListAsync();

            var cartPaymentMethods = new List<CartPaymentMethod>();

            foreach (PaymentMethod method in methods)
            {
                cartPaymentMethods.Add(new CartPaymentMethod()
                {
                    PaymentMethod = method,
                    Amount = await _db
                        .CartPaymentMethods
                        .Where(x => x.Cart.RouteID == routeID && x.PaymentMethodID == method.ID)
                        .SumAsync(x => x.Amount)
                });
            }
            return cartPaymentMethods;
        }

        public async Task<List<Client>> ClientsInRoute(long routeID)
        {
            return await _db
                .Carts
                .Where(x => x.RouteID == routeID && x.IsStatic)
                .OrderBy(x => x.Priority)
                .Select(x => x.Client)
                .ToListAsync();
        }

        public async Task<List<Client>> ClientsNotInRoute(long routeID)
        {
            return await _db
                .Clients
                .Where(x => x.IsActive && !x.Carts.Any(x => x.Route.ID == routeID))
                .ToListAsync();
        }

        public async Task<long> CreateByDealer(long routeID)
        {
            var route = await _db
                .Routes
                .Include(x => x.Carts)
                .FirstAsync(x => x.ID == routeID && x.IsStatic) ?? throw new Exception("No se ha encontrado la planilla");
            
            var newRoute = new Models.Route()
            {
                UserID = route.UserID,
                DayOfWeek = route.DayOfWeek,
                IsStatic = false,
            };
            await _db.Routes.AddAsync(newRoute);

            foreach (var cart in route.Carts)
            {
                var newCart = new Cart()
                {
                    ClientID = cart.ClientID,
                    Route = newRoute,
                    Priority = cart.Priority,
                    State = State.Pending,
                    IsStatic = false,
                };
                await _db.Carts.AddAsync(newCart);
            }
            try
            {
                await _db.Database.BeginTransactionAsync();
                await _db.SaveChangesAsync();
                await _db.Database.CommitTransactionAsync();
                return newRoute.ID;
            }
            catch (Exception)
            {
                await _db.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateDispatched(long routeID, List<DispatchedProduct> products)
        {

            var route = await _db
                .Routes
                .Include(x => x.DispatchedProducts)
                .FirstAsync(x => x.ID == routeID) ?? throw new Exception("No se ha encontrado la planilla");


            foreach (var dispatchedProduct in route.DispatchedProducts)
            {
                dispatchedProduct.DeletedAt = DateTime.UtcNow.AddHours(-3);
            }

            var oldProducts = await _db
                .DispatchedProducts
                .IgnoreQueryFilters()
                .Where(x => x.RouteID == routeID)
                .ToListAsync();

            foreach (var dispatchedProduct in products)
            {
                if (oldProducts.Any(x => x.Type == dispatchedProduct.Type))
                {
                    var oldProduct = oldProducts.First(x => x.Type == dispatchedProduct.Type);
                    oldProduct.DeletedAt = null;
                    oldProduct.Quantity = dispatchedProduct.Quantity;
                    oldProduct.UpdatedAt = DateTime.UtcNow.AddHours(-3);
                }
                else
                {
                    await _db.DispatchedProducts.AddAsync(new()
                    {
                        RouteID = routeID,
                        Type = dispatchedProduct.Type,
                        Quantity = dispatchedProduct.Quantity,
                    });
                    
                }
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
    }
}