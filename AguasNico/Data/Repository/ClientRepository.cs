using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using Microsoft.EntityFrameworkCore;
using AguasNico.Models.ViewModels.Clients;
using AguasNico.Models.ViewModels.Tables;

namespace AguasNico.Data.Repository
{
    public class ClientRepository(ApplicationDbContext db) : Repository<Client>(db), IClientRepository
    {
        private readonly ApplicationDbContext _db = db;

        public void Update(Client client)
        {
            try
            {
                _db.Database.BeginTransaction();

                var dbObject = _db.Clients.First(x => x.ID == client.ID) ?? throw new Exception("No se ha encontrado el cliente");
                dbObject.Name = client.Name;
                dbObject.Address = client.Address;
                dbObject.Phone = client.Phone;
                dbObject.Observations = client.Observations;
                dbObject.Debt = client.Debt;
                dbObject.HasInvoice = client.HasInvoice;
                dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);

                if (dbObject.DealerID != client.DealerID || dbObject.DeliveryDay != client.DeliveryDay)
                {
                    Cart? cart = _db.Carts.Where(x => x.ClientID == client.ID && x.IsStatic == true).FirstOrDefault();
                    if (cart is not null)
                    {
                        cart.DeletedAt = DateTime.UtcNow.AddHours(-3);
                    }
                    Models.Route? route = _db.Routes.Where(x => x.UserID == client.DealerID && x.DayOfWeek == client.DeliveryDay && x.IsStatic).Include(x => x.Carts).FirstOrDefault();
                    if (route is not null)
                    {
                        _db.Carts.Add(new()
                        {
                            RouteID = route.ID,
                            ClientID = client.ID,
                            IsStatic = true,
                            State = State.Pending,
                            Priority = route.Carts.Any() ? route.Carts.Max(x => x.Priority) + 1 : 1,
                        });
                    }
                    dbObject.DealerID = client.DealerID;
                    dbObject.DeliveryDay = client.DeliveryDay;
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

        public IEnumerable<ClientProduct> GetProducts(long clientID)
        {
            return _db.ClientProducts.Where(x => x.ClientID == clientID);
        }

        public IEnumerable<ClientProduct> GetAllProducts(long clientID)
        {
            IEnumerable<Product> products = _db.Products.OrderBy(x => x.Name).ThenBy(x => x.Price).Where(x => x.IsActive);
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
            return _db.Clients.Any(x => x.Name == client.Name && x.Address == client.Address && x.ID != client.ID);
        }

        public void SoftDelete(long id)
        {
            try
            {
                _db.Database.BeginTransaction();
                var dbObject = _db.Clients.Include(x => x.Carts).Include(x => x.Products).Include(x => x.Abonos).First(x => x.ID == id) ?? throw new Exception("No se ha encontrado el cliente");
                dbObject.IsActive = false;
                foreach (Cart cart in dbObject.Carts.Where(x => x.IsStatic))
                {
                    cart.DeletedAt = DateTime.UtcNow.AddHours(-3);
                }

                foreach (ClientProduct clientProduct in dbObject.Products)
                {
                    clientProduct.DeletedAt = DateTime.UtcNow.AddHours(-3);
                }

                foreach (ClientAbono abono in dbObject.Abonos)
                {
                    abono.DeletedAt = DateTime.UtcNow.AddHours(-3);
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

        public void AddProducInTransaction(ClientProduct clientProduct)
        {
            _db.ClientProducts.Add(clientProduct);
        }

        public void UpdateInvoiceData(Client client)
        {
            var dbObject = _db.Clients.First(x => x.ID == client.ID) ?? throw new Exception("No se ha encontrado el cliente");
            dbObject.InvoiceType = client.InvoiceType;
            dbObject.TaxCondition = client.TaxCondition;
            dbObject.CUIT = client.CUIT;
            dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            _db.SaveChanges();
        }

        public void UpdateProducts(long clientID, List<ClientProduct> products)
        {
            try
            {
                _db.Database.BeginTransaction();

                IEnumerable<ClientProduct> clientProducts = _db.ClientProducts.IgnoreQueryFilters().Where(x => x.ClientID == clientID);
                foreach (ClientProduct product in products)
                {
                    if (products.Any(x => x.Product.Type == product.Product.Type && x.ProductID != product.ProductID))
                    {
                        throw new Exception("No se pueden agregar dos productos del mismo tipo");
                    }

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
                        _db.ClientProducts.Add(new()
                        {
                            ClientID = clientID,
                            ProductID = product.ProductID,
                            Stock = product.Stock,
                        });
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

        public IEnumerable<ProductHistory> GetProductsHistory(long clientID)
        {
            IEnumerable<CartProduct> soldProducts = _db.CartProducts
                .Include(x => x.Cart)
                .Where(x => x.Cart.ClientID == clientID && (x.Type != ProductType.Máquina))
                .OrderByDescending(x => x.CreatedAt)
                .Take(10);
            IEnumerable<ReturnedProduct> returnedProducts = _db.ReturnedProducts
                .Include(x => x.Cart)
                .Where(x => x.Cart.ClientID == clientID && (x.Type != ProductType.Máquina))
                .OrderByDescending(x => x.CreatedAt)
                .Take(10);

            List<ProductHistory> productsHistory = [];
            foreach (CartProduct soldProduct in soldProducts)
            {
                ProductHistory product = new()
                {
                    ProductType = soldProduct.Type,
                    ActionType = ActionType.Baja,
                    Quantity = soldProduct.Quantity,
                    Date = soldProduct.CreatedAt,
                };
                productsHistory.Add(product);
            }
            foreach (ReturnedProduct returnedProduct in returnedProducts)
            {
                ProductHistory product = new()
                {
                    ProductType = returnedProduct.Type,
                    ActionType = ActionType.Devuelve,
                    Quantity = returnedProduct.Quantity,
                    Date = returnedProduct.CreatedAt,
                };
                productsHistory.Add(product);
            }
            return productsHistory.OrderByDescending(x => x.Date);
        }

        public IEnumerable<Client> GetNotVisited(DateTime dateFrom, DateTime dateTo, string dealerID)
        {
            return _db.Clients
                .Include(x => x.Carts)
                .Where(x => x.DealerID == dealerID && x.Carts.Any(y => !y.IsStatic && y.CreatedAt.DayOfYear >= dateFrom.DayOfYear && y.CreatedAt.DayOfYear <= dateTo.DayOfYear && y.State != State.Confirmed))
                .OrderBy(x => x.Name)
                .Select(x => new Client
                {
                    ID = x.ID,
                    Name = x.Name,
                    Address = x.Address,
                });
        }

        public List<CartsTransfersHistoryTable> GetCartsTransfersHistoryTable(long clientID)
        {
            TransferRepository transferRepository = new(_db);
            IEnumerable<Transfer> transfers = transferRepository.GetLastTen(clientID);
            CartRepository cartRepository = new(_db);
            IEnumerable<Cart> carts = cartRepository.GetLastTen(clientID);
            
            List<CartsTransfersHistoryTable> cartsTransfersHistory = [];

            foreach (Transfer transfer in transfers)
            {
                cartsTransfersHistory.Add(new()
                {
                    Date = transfer.Date,
                    Type = CartsTransfersType.Transfer,
                    TransferAmount = transfer.Amount,
                });
            }

            foreach (Cart cart in carts)
            {
                List<CartPaymentMethod> payments = cart.PaymentMethods.ToList();
                List<CartProduct> products = cart.Products.ToList();

                cartsTransfersHistory.Add(new()
                {
                    Date = cart.CreatedAt,
                    Type = CartsTransfersType.Cart,
                    PaymentMethods = payments,
                    Products = products,
                });
            }
            return [.. cartsTransfersHistory.OrderByDescending(x => x.Date)];
        }
    }
}