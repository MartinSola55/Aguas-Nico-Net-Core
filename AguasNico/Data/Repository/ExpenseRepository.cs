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
    public class ExpenseRepository(ApplicationDbContext db) : Repository<Expense>(db), IExpenseRepository
    {
        private readonly ApplicationDbContext _db = db;

        public async Task Update(Expense expense)
        {
            var oldExpense = await _db
                .Expenses
                .FirstAsync(x => x.ID == expense.ID) ?? throw new Exception("No se ha encontrado el gasto");

            oldExpense.UserID = expense.UserID;
            oldExpense.Amount = expense.Amount;
            oldExpense.UpdatedAt = DateTime.UtcNow.AddHours(-3);

            await _db.SaveChangesAsync();
        }

        public async Task SoftDelete(long id)
        {
            var oldExpense = await _db
                .Expenses
                .FirstAsync(x => x.ID == id) ?? throw new Exception("No se ha encontrado el gasto");

            oldExpense.DeletedAt = DateTime.UtcNow.AddHours(-3);
            await _db.SaveChangesAsync();
        }

        public async Task<decimal> GetTotalExpenses(DateTime date)
        {
            return await _db
                .Expenses
                .Where(x => x.CreatedAt.Date == date.Date)
                .SumAsync(x => x.Amount);
        }

        public async Task<decimal> GetTotalExpensesByDealer(DateTime date, string dealerID)
        {
            return await _db
                .Expenses
                .Where(x => x.CreatedAt.Date == date.Date && x.UserID == dealerID)
                .SumAsync(x => x.Amount);
        }
    }
}