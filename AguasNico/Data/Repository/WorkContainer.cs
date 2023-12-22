using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AguasNico.Data.Repository.IRepository;

namespace AguasNico.Data.Repository
{
    public class WorkContainer : IWorkContainer
    {
        private readonly ApplicationDbContext _db;

        public WorkContainer(ApplicationDbContext db)
        {
            _db = db;
            ApplicationUser = new ApplicationUserRepository(_db);
            Product = new ProductRepository(_db);
            Client = new ClientRepository(_db);
            Transfer = new TransferRepository(_db);
            Expense = new ExpenseRepository(_db);
        }

        public IApplicationUserRepository ApplicationUser { get; private set; }

        public IProductRepository Product { get; private set; }

        public IClientRepository Client { get; private set; }

        public ITransferRepository Transfer { get; private set; }

        public IExpenseRepository Expense { get; private set; }


        public void BeginTransaction()
        {
            _db.Database.BeginTransaction();
        }

        public void Commit()
        {
            _db.Database.CommitTransaction();
        }

        public void Rollback()
        {
            _db.Database.RollbackTransaction();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}