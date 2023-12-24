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
    public class ClientRepository(ApplicationDbContext db) : Repository<Client>(db), IClientRepository
    {
        private readonly ApplicationDbContext _db = db;

        public void Update(Client client)
        {
            var dbObject = _db.Clients.First(x => x.ID == client.ID) ?? throw new Exception("No se ha encontrado el cliente");
            dbObject.Name = client.Name;
            dbObject.Address = client.Address;
            dbObject.Phone = client.Phone;
            dbObject.Observations = client.Observations;
            dbObject.Debt = client.Debt;
            dbObject.HasInvoice = client.HasInvoice;
            dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            _db.SaveChanges();
        }

        public IEnumerable<ClientProduct> GetProducts(long clientID)
        {
            return _db.ClientProducts.Where(x => x.ClientID == clientID);
        }

        public IEnumerable<ClientProduct> GetAllProducts(long clientID)
        {
            IEnumerable<Product> products = _db.Products.OrderBy(x => x.Name).ThenBy(x => x.Price);
            List<ClientProduct> clientProducts = [.. _db.ClientProducts.Where(x => x.ClientID == clientID)];
            foreach (Product product in products)
            {
                if (!clientProducts.Any(x => x.ProductID == product.ID))
                {
                    clientProducts.Add(new ClientProduct
                    {
                        ClientID = clientID,
                        Product = product,
                        ProductID = product.ID,
                        Stock = -1,
                    });
                }
            }
            return clientProducts;
        }

        public bool IsDuplicated(Client client)
        {
            return _db.Clients.Any(x => x.Name == client.Name && x.Address == client.Address);
        }

        public void SoftDelete(long id)
        {
            var dbObject = _db.Clients.First(x => x.ID == id) ?? throw new Exception("No se ha encontrado el cliente");
            dbObject.DeletedAt = DateTime.UtcNow.AddHours(-3);
            _db.SaveChanges();
        }

        public void AddProducInTransaction(ClientProduct clientProduct)
        {
            _db.ClientProducts.Add(clientProduct);
        }

        public void UpdateProducts(long clientID, List<ClientProduct> products)
        {
            try
            {
                _db.Database.BeginTransaction();
                IEnumerable<ClientProduct> clientProducts = _db.ClientProducts.IgnoreQueryFilters().Where(x => x.ClientID == clientID);
                foreach (ClientProduct product in products)
                {
                    if (clientProducts.Any(x => x.ProductID == product.ProductID))
                    {
                        ClientProduct clientProduct = clientProducts.First(x => x.ProductID == product.ProductID);
                        clientProduct.Stock = product.Stock;
                        clientProduct.UpdatedAt = DateTime.UtcNow.AddHours(-3);
                        clientProduct.DeletedAt = null;
                        _db.ClientProducts.Update(clientProduct);
                    }
                    else
                    {
                        _db.ClientProducts.Add(product);
                    }
                }

                // Actualizar DeletedAt para los productos que no se encuentran en la lista de productos nuevos
                IEnumerable<ClientProduct> existingProducts = clientProducts.Where(x => !products.Any(y => y.ProductID == x.ProductID));

                if (existingProducts.Any())
                {
                    foreach (ClientProduct existingProduct in existingProducts)
                    {
                        existingProduct.DeletedAt = DateTime.UtcNow.AddHours(-3);
                    }
                    _db.ClientProducts.UpdateRange(existingProducts);
                }
                _db.SaveChanges();
                _db.Database.CommitTransaction();
            }
            catch (Exception)
            {
                _db.Database.RollbackTransaction();
                throw;
            }
        }
    }
}