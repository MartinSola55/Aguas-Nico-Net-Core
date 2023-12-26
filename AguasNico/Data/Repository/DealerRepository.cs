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
    public class DealerRepository(ApplicationDbContext db) : Repository<ApplicationUser>(db), IDealerRepository
    {
        private readonly ApplicationDbContext _db = db;

        public int GetTotalCarts(string dealerID, DateTime date)
        {
            return _db.Carts
                .Where(x => x.Route.UserID == dealerID && x.CreatedAt.Month == date.Month && x.CreatedAt.Year == date.Year)
                .Count();
        }

        public decimal GetTotalCollected(string dealerID, DateTime date)
        {
            return _db.CartPaymentMethods
                .Where(x => x.Cart.Route.UserID == dealerID && x.CreatedAt.Month == date.Month && x.CreatedAt.Year == date.Year)
                .Sum(x => x.Amount);
        }

        public int GetTotalCompletedCarts(string dealerID, DateTime date)
        {
            return _db.Carts
                .Where(x => x.Route.UserID == dealerID && x.State == State.Confirmed && x.CreatedAt.Month == date.Month && x.CreatedAt.Year == date.Year)
                .Count();
        }

        public int GetTotalPendingCarts(string dealerID, DateTime date)
        {
            return _db.Carts
                .Where(x => x.Route.UserID == dealerID && x.State != State.Confirmed && x.CreatedAt.Month == date.Month && x.CreatedAt.Year == date.Year)
                .Count();
        }
    }
}