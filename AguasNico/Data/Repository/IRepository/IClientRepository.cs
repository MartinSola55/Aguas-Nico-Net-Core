using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Models;
using AguasNico.Models.ViewModels.Clients;
using AguasNico.Models.ViewModels.Tables;

namespace AguasNico.Data.Repository.IRepository
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<bool> Create(Client client);
        Task Update(Client client);
        Task<bool> IsDuplicated(Client client);

        /// <summary>
        /// Obtiene todos los productos en stock del cliente.
        /// </summary>
        /// <param name="clientID">ID del cliente</param>
        /// <returns>Una colección de los productos en stock del cliente</returns>
        Task<List<ClientProduct>> GetProducts(long clientID);

        /// <summary>
        /// Obtiene todos los productos del sistema, y asigna el stock del cliente a cada uno.
        /// </summary>
        /// <param name="clientID">ID del cliente</param>
        /// <returns>Una colección de todos los productos, con el stock del cliente en cada uno</returns>
        Task<List<ClientProduct>> GetAllProducts(long clientID);
        Task SoftDelete(long id);

        /// <summary>
        /// Adds a client product in a transaction.
        /// </summary>
        /// <param name="clientProduct">The client product to add.</param>
        Task AddProducInTransaction(ClientProduct clientProduct);

        Task UpdateInvoiceData(Client client);

        Task UpdateProducts(long clientID, List<ClientProduct> clientProduct);
        Task UpdateAbonos(long clientID, List<ClientAbono> abonos);
        Task<List<ClientAbono>> GetAbonos(long clientID);
        Task<List<AbonoRenewalProduct>> GetAbonosRenewedAvailables(long clientID);
        Task<List<ProductHistory>> GetProductsHistory(long clientID);
        Task<List<Client>> GetNotVisited(DateTime dateFrom, DateTime dateTo, string dealerID);
        Task<List<CartsTransfersHistoryTable>> GetCartsTransfersHistoryTable(long clientID);
        Task<List<Client>> GetUnassignedClients();
    }
}