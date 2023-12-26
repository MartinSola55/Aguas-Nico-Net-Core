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
        void Update(Expense expense);
        void SoftDelete(long id);
        decimal GetTotalExpenses(DateTime date);
        decimal GetTotalExpensesByDealer(DateTime date, string dealerID);
    }
}