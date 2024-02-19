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
            Route = new RouteRepository(_db);
            PaymentMethod = new PaymentMethodRepository(_db);
            DispatchedProduct = new DispatchedProductRepository(_db);
            Cart = new CartRepository(_db);
            Tables = new TablesRepository(_db);
            Dealer = new DealerRepository(_db);
            Abono = new AbonoRepository(_db);
        }

        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IProductRepository Product { get; private set; }
        public IClientRepository Client { get; private set; }
        public ITransferRepository Transfer { get; private set; }
        public IExpenseRepository Expense { get; private set; }
        public IRouteRepository Route { get; private set; }
        public IPaymentMethodRepository PaymentMethod { get; private set; }
        public IDispatchedProductRepository DispatchedProduct { get; private set; }
        public ICartRepository Cart { get; private set; }
        public ITablesRepository Tables { get; private set; }
        public IDealerRepository Dealer { get; private set; }
        public IAbonoRepository Abono { get; private set; }

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

        public async Task BeginTransactionAsync()
        {
            await _db.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _db.Database.CommitTransactionAsync();
        }

        public async Task RollbackAsync()
        {
            await _db.Database.RollbackTransactionAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}