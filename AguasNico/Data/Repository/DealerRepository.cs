using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using Microsoft.EntityFrameworkCore;
using AguasNico.Models.ViewModels.Tables;

namespace AguasNico.Data.Repository
{
    public class DealerRepository(ApplicationDbContext db) : Repository<ApplicationUser>(db), IDealerRepository
    {
        private readonly ApplicationDbContext _db = db;

        public async Task<int> GetTotalCarts(string dealerID, DateTime date)
        {
            return await _db
                .Carts
                .Where(x => x.Route.UserID == dealerID && x.CreatedAt.Month == date.Month && x.CreatedAt.Year == date.Year && !x.IsStatic)
                .CountAsync();
        }

        public async Task<decimal> GetTotalCollected(string dealerID, DateTime date)
        {
            return await _db
                .CartPaymentMethods
                .Where(x => x.Cart.Route.UserID == dealerID && x.CreatedAt.Month == date.Month && x.CreatedAt.Year == date.Year)
                .SumAsync(x => x.Amount);
        }

        public async Task<int> GetTotalCompletedCarts(string dealerID, DateTime date)
        {
            return await _db
                .Carts
                .Where(x => x.Route.UserID == dealerID && x.State == State.Confirmed && x.CreatedAt.Month == date.Month && x.CreatedAt.Year == date.Year && !x.IsStatic)
                .CountAsync();
        }

        public async Task<int> GetTotalPendingCarts(string dealerID, DateTime date)
        {
            return await _db
                .Carts
                .Where(x => x.Route.UserID == dealerID && x.State != State.Confirmed && x.CreatedAt.Month == date.Month && x.CreatedAt.Year == date.Year && !x.IsStatic)
                .CountAsync();
        }

        public async Task<List<DealerSheet>> GetDealerSheet(string dealerID, Day day)
        {
            var dealer = await _db.User
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == dealerID) ?? throw new Exception("No se ha encontrado al repartidor");

            var staticCarts = await _db
                .Carts
                .Include(x => x.Client)
                    .ThenInclude(x => x.Products)
                        .ThenInclude(x => x.Product)
                .Include(x => x.Client)
                    .ThenInclude(x => x.AbonosRenewed)
                        .ThenInclude(x => x.ProductsAvailables)
                .Where(x => x.Route.UserID == dealerID && x.IsStatic && x.Route.DayOfWeek == day)
                .ToListAsync();

            var sheets = new List<DealerSheet>();
            var today = DateTime.UtcNow.AddHours(-3);
            foreach (var cart in staticCarts)
            {
                var dealerSheet = new DealerSheet
                {
                    ClientName = cart.Client.Name,
                    ClientPhone = cart.Client.Phone,
                    ClientAddress = cart.Client.Address,
                    ClientDebt = cart.Client.Debt,
                    Products = cart.Client.Products.Select(x => new DealerSheet.Product
                    {
                        Type = x.Product.Type,
                        Price = x.Product.Price,
                        Stock = x.Stock,
                    }).ToList(),
                    AbonoProducts = cart.Client.AbonosRenewed
                    .Where(x => x.CreatedAt.Month == today.Month && x.CreatedAt.Year == today.Year)
                    .SelectMany(x => x.ProductsAvailables)
                    .Select(x => new DealerSheet.AbonoProduct
                    {
                        Type = x.Type,
                        Available = x.Available,
                    }).ToList()
                };

                sheets.Add(dealerSheet);
            }

            return sheets;
        }
    }
}