using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;

namespace AguasNico.Data.Repository
{
    public class ExpenseRepository(ApplicationDbContext db) : Repository<Expense>(db), IExpenseRepository
    {
        private readonly ApplicationDbContext _db = db;

        public void Update(Expense expense)
        {
            var dbObject = _db.Expenses.First(x => x.ID == expense.ID) ?? throw new Exception("No se ha encontrado el gasto");
            dbObject.UserID = expense.UserID;
            dbObject.Amount = expense.Amount;
            dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            _db.SaveChanges();
        }

        public void SoftDelete(long id)
        {
            try
            {
                var dbObject = _db.Expenses.First(x => x.ID == id) ?? throw new Exception("No se ha encontrado el gasto");
                dbObject.DeletedAt = DateTime.UtcNow.AddHours(-3);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public decimal GetTotalExpenses(DateTime date)
        {
            return _db.Expenses
                .Where(x => x.CreatedAt.Date == date.Date)
                .Sum(x => x.Amount);
        }
    }
}