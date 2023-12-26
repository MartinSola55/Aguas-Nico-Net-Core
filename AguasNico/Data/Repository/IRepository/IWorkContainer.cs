using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AguasNico.Data.Repository.IRepository
{
    public interface IWorkContainer : IDisposable
    {
        // Agregar los repositorios
        IApplicationUserRepository ApplicationUser { get; }
        IProductRepository Product { get; }
        IClientRepository Client { get; }
        ITransferRepository Transfer { get; }
        IExpenseRepository Expense { get; }
        IRouteRepository Route { get; }
        IPaymentMethodRepository PaymentMethod { get; }
        IDispatchedProductRepository DispatchedProduct { get; }
        ICartRepository Cart { get; }
        ITablesRepository Tables { get; }
        IDealerRepository Dealer { get; }

        void BeginTransaction();
        void Commit();
        void Rollback();
        void Save();
    }
}