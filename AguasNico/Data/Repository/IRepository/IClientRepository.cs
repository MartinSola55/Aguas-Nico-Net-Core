using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Models;
using AguasNico.Models.ViewModels.Clients;

namespace AguasNico.Data.Repository.IRepository
{
    public interface IClientRepository : IRepository<Client>
    {
        void Update(Client client);
        bool IsDuplicated(Client client);

        /// <summary>
        /// Obtiene todos los productos en stock del cliente.
        /// </summary>
        /// <param name="clientID">ID del cliente</param>
        /// <returns>Una colección de los productos en stock del cliente</returns>
        IEnumerable<ClientProduct> GetProducts(long clientID);

        /// <summary>
        /// Obtiene todos los productos del sistema, y asigna el stock del cliente a cada uno.
        /// </summary>
        /// <param name="clientID">ID del cliente</param>
        /// <returns>Una colección de todos los productos, con el stock del cliente en cada uno</returns>
        IEnumerable<ClientProduct> GetAllProducts(long clientID);
        void SoftDelete(long id);

        /// <summary>
        /// Adds a client product in a transaction.
        /// </summary>
        /// <param name="clientProduct">The client product to add.</param>
        void AddProducInTransaction(ClientProduct clientProduct);

        void UpdateInvoiceData(Client client);

        void UpdateProducts(long clientID, List<ClientProduct> clientProduct);
        IEnumerable<ProductHistory> GetProductsHistory(long clientID);
        IEnumerable<Client> GetNotVisited(DateTime dateFrom, DateTime dateTo, string dealerID);
    }
}