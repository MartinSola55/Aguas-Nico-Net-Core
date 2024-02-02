using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Models;

namespace AguasNico.Data.Repository.IRepository
{
    public interface IExpenseRepository : IRepository<Expense>
    {
        Task Update(Expense expense);
        Task SoftDelete(long id);
        Task<decimal> GetTotalExpenses(DateTime date);
        Task<decimal> GetTotalExpensesByDealer(DateTime date, string dealerID);
    }
}