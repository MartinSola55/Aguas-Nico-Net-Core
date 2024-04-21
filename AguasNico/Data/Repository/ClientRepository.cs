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
using NuGet.Protocol;

namespace AguasNico.Data.Repository
{
    public class ClientRepository(ApplicationDbContext db) : Repository<Client>(db), IClientRepository
    {
        private readonly ApplicationDbContext _db = db;

        private static bool ValidateProducts(List<Product> products)
        {
            foreach (var product in products)
            {
                if (products.Any(x => x.Type == product.Type && x.ID != product.ID))
                    return false;
            }
            return true;
        }

        public async Task<bool> Create(Client client)
        {
            var products = await _db.Products.Where(x => client.Products.Select(y => y.ProductID).Contains(x.ID)).ToListAsync();
            if (!ValidateProducts(products))
                return false;

            await _db.Clients.AddAsync(client);

            if (client.DealerID is not null && client.DeliveryDay is not null)
            {
                var route = await _db
                    .Routes
                    .Include(x => x.Carts)
                    .FirstOrDefaultAsync(x => x.UserID == client.DealerID && x.DayOfWeek == client.DeliveryDay);

                if (route is not null)
                {
                    int priority = route.Carts.Any() ? route.Carts.Max(x => x.Priority) + 1 : 1;
                    var cart = new Cart
                    {
                        RouteID = route.ID,
                        Client = client,
                        Priority = priority,
                        State = State.Pending,
                        IsStatic = true,
                    };
                    await _db.Carts.AddAsync(cart);
                }
            }

            try
            {
                await _db.Database.BeginTransactionAsync();
                await _db.SaveChangesAsync();
                await _db.Database.CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                await _db.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task Update(Client client)
        {
            var oldClient = await _db
                .Clients
                .FirstAsync(x => x.ID == client.ID) ?? throw new Exception("No se ha encontrado el cliente");

            oldClient.Name = client.Name;
            oldClient.Address = client.Address;
            oldClient.Phone = client.Phone;
            oldClient.Observations = client.Observations;
            oldClient.Debt = client.Debt;
            oldClient.HasInvoice = client.HasInvoice;
            oldClient.OnlyAbonos = client.OnlyAbonos;
            oldClient.UpdatedAt = DateTime.UtcNow.AddHours(-3);

            if (oldClient.DealerID != client.DealerID || oldClient.DeliveryDay != client.DeliveryDay)
            {
                var cart = await _db
                    .Carts
                    .Where(x => x.ClientID == client.ID && x.IsStatic == true)
                    .FirstOrDefaultAsync();

                if (cart is not null)
                {
                    cart.DeletedAt = DateTime.UtcNow.AddHours(-3);
                }
                var route = await _db
                    .Routes
                    .Where(x => x.UserID == client.DealerID && x.DayOfWeek == client.DeliveryDay && x.IsStatic)
                    .Include(x => x.Carts)
                    .FirstOrDefaultAsync();

                if (route is not null)
                {
                    await _db.Carts.AddAsync(new()
                    {
                        RouteID = route.ID,
                        ClientID = client.ID,
                        IsStatic = true,
                        State = State.Pending,
                        Priority = route.Carts.Any() ? route.Carts.Max(x => x.Priority) + 1 : 1,
                    });
                }
                // The update is done here because the client MUST have a cart assinged
                oldClient.DealerID = client.DealerID;
                oldClient.DeliveryDay = client.DeliveryDay;
            }
            try
            {
                await _db.Database.BeginTransactionAsync();
                await _db.SaveChangesAsync();
                await _db.Database.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _db.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<ClientProduct>> GetProducts(long clientID)
        {
            return await _db
                .ClientProducts
                .Where(x => x.ClientID == clientID)
                .ToListAsync();
        }

        public async Task<List<ClientProduct>> GetAllProducts(long clientID)
        {
            var products = await _db
                .Products
                .OrderBy(x => x.Name)
                    .ThenBy(x => x.Price)
                .Where(x => x.IsActive)
                .ToListAsync();

            var clientProducts = await _db
                .ClientProducts
                .Where(x => x.ClientID == clientID)
                .ToListAsync();

            foreach (var product in products)
            {
                if (!clientProducts.Any(x => x.ProductID == product.ID))
                {
                    clientProducts.Add(new()
                    {
                        ClientID = clientID,
                        ProductID = product.ID,
                        Product = product,
                        Stock = -1,
                    });
                }
            }
            return clientProducts.OrderBy(x => x.Product.SortOrder).ThenBy(x => x.Product.Name).ThenBy(x => x.Product.Price).ToList();
        }

        public async Task<bool> IsDuplicated(Client client)
        {
            return await _db
                .Clients
                .AnyAsync(x => x.Name == client.Name && x.Address == client.Address && x.ID != client.ID);
        }

        public async Task SoftDelete(long id)
        {

            var client = await _db
                .Clients
                .Include(x => x.Carts)
                .Include(x => x.Products)
                .Include(x => x.Abonos)
                .Include(x => x.Transfers)
                .Include(x => x.AbonosRenewed)
                .FirstAsync(x => x.ID == id) ?? throw new Exception("No se ha encontrado el cliente");

            client.IsActive = false;
            foreach (var cart in client.Carts.Where(x => x.IsStatic))
            {
                cart.DeletedAt = DateTime.UtcNow.AddHours(-3);
            }

            foreach (var clientProduct in client.Products)
            {
                clientProduct.DeletedAt = DateTime.UtcNow.AddHours(-3);
            }

            foreach (var abono in client.Abonos)
            {
                abono.DeletedAt = DateTime.UtcNow.AddHours(-3);
            }

            foreach (var transfer in client.Transfers)
            {
                transfer.DeletedAt = DateTime.UtcNow.AddHours(-3);
            }

            foreach (var abonoRenewed in client.AbonosRenewed)
            {
                abonoRenewed.DeletedAt = DateTime.UtcNow.AddHours(-3);
            }
            try
            {
                await _db.Database.BeginTransactionAsync();
                await _db.SaveChangesAsync();
                await _db.Database.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _db.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task AddProducInTransaction(ClientProduct clientProduct)
        {
            await _db.ClientProducts.AddAsync(clientProduct);
        }

        public async Task UpdateInvoiceData(Client client)
        {
            var oldClient = await _db
                .Clients
                .FirstAsync(x => x.ID == client.ID) ?? throw new Exception("No se ha encontrado el cliente");

            oldClient.InvoiceType = client.InvoiceType;
            oldClient.TaxCondition = client.TaxCondition;
            oldClient.CUIT = client.CUIT;
            oldClient.UpdatedAt = DateTime.UtcNow.AddHours(-3);

            await _db.SaveChangesAsync();
        }

        public async Task UpdateProducts(long clientID, List<ClientProduct> products)
        {
            var clientProducts = await _db
                .ClientProducts
                .IgnoreQueryFilters()
                .Where(x => x.ClientID == clientID)
                .ToListAsync();

            foreach (var product in products)
            {
                if (products.Any(x => x.Product.Type == product.Product.Type && x.ProductID != product.ProductID))
                    throw new Exception("No se pueden agregar dos productos del mismo tipo");

                if (clientProducts.Any(x => x.ProductID == product.ProductID))
                {
                    var clientProduct = clientProducts.First(x => x.ProductID == product.ProductID);
                    clientProduct.Stock = product.Stock;
                    clientProduct.UpdatedAt = DateTime.UtcNow.AddHours(-3);
                    clientProduct.DeletedAt = null;
                }
                else
                {
                    await _db.ClientProducts.AddAsync(new()
                    {
                        ClientID = clientID,
                        ProductID = product.ProductID,
                        Stock = product.Stock,
                    });
                }
            }

            // Actualizar DeletedAt para los productos que no se encuentran en la lista de productos nuevos
            var existingProducts = clientProducts.Where(x => !products.Any(y => y.ProductID == x.ProductID));

            if (existingProducts.Any())
            {
                foreach (var existingProduct in existingProducts)
                {
                    existingProduct.DeletedAt = DateTime.UtcNow.AddHours(-3);
                }
            }
            try
            {
                await _db.Database.BeginTransactionAsync();
                await _db.SaveChangesAsync();
                await _db.Database.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _db.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateAbonos(long clientID, List<ClientAbono> abonos)
        {
            var clientAbonos = await _db
                .ClientAbonos
                .IgnoreQueryFilters()
                .Where(x => x.ClientID == clientID)
                .ToListAsync();

            foreach (var abono in abonos)
            {
                if (clientAbonos.Any(x => x.AbonoID == abono.AbonoID))
                {
                    var clientAbono = clientAbonos.First(x => x.AbonoID == abono.AbonoID);
                    clientAbono.UpdatedAt = DateTime.UtcNow.AddHours(-3);
                    clientAbono.DeletedAt = null;
                }
                else
                {
                    await _db.ClientAbonos.AddAsync(new()
                    {
                        ClientID = clientID,
                        AbonoID = abono.AbonoID
                    });
                }
            }

            // Actualizar DeletedAt para los productos que no se encuentran en la lista de productos nuevos
            var existingAbonos = clientAbonos.Where(x => !abonos.Any(y => y.AbonoID == x.AbonoID));

            if (existingAbonos.Any())
            {
                foreach (var existingAbono in existingAbonos)
                {
                    existingAbono.DeletedAt = DateTime.UtcNow.AddHours(-3);
                }
            }
            try
            {
                await _db.Database.BeginTransactionAsync();
                await _db.SaveChangesAsync();
                await _db.Database.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _db.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<ProductHistory>> GetProductsHistory(long clientID)
        {
            List<CartProduct> soldProducts = await _db
                .CartProducts
                .Include(x => x.Cart)
                .Where(x => x.Cart.ClientID == clientID && (x.Type != ProductType.Máquina))
                .OrderByDescending(x => x.CreatedAt)
                .Take(10)
                .ToListAsync();

            List<CartAbonoProduct> abonoProducts = await _db
                .CartAbonoProducts
                .Include(x => x.Cart)
                .Where(x => x.Cart.ClientID == clientID && (x.Type != ProductType.Máquina))
                .OrderByDescending(x => x.CreatedAt)
                .Take(10)
                .ToListAsync();

            List<ReturnedProduct> returnedProducts = await _db
                .ReturnedProducts
                .Include(x => x.Cart)
                .Where(x => x.Cart.ClientID == clientID && (x.Type != ProductType.Máquina))
                .OrderByDescending(x => x.CreatedAt)
                .Take(10)
                .ToListAsync();

            var productsHistory = new List<ProductHistory>();
            foreach (var soldProduct in soldProducts)
            {
                productsHistory.Add(new()
                {
                    ProductType = soldProduct.Type,
                    ActionType = ActionType.Baja,
                    Quantity = soldProduct.Quantity,
                    Date = soldProduct.CreatedAt,
                });
            }

            foreach (var abonoProduct in abonoProducts)
            {
                productsHistory.Add(new()
                {
                    ProductType = abonoProduct.Type,
                    ActionType = ActionType.Abono,
                    Quantity = abonoProduct.Quantity,
                    Date = abonoProduct.CreatedAt,
                });
            }

            foreach (var returnedProduct in returnedProducts)
            {
                productsHistory.Add(new()
                {
                    ProductType = returnedProduct.Type,
                    ActionType = ActionType.Devuelve,
                    Quantity = returnedProduct.Quantity,
                    Date = returnedProduct.CreatedAt,
                });
            }
            return [.. productsHistory.OrderByDescending(x => x.Date)];
        }

        public async Task<List<Client>> GetNotVisited(DateTime dateFrom, DateTime dateTo, string dealerID)
        {
            return await _db
                .Clients
                .Include(x => x.Carts)
                .Where(x =>
                    x.IsActive &&
                    x.DealerID == dealerID &&
                    x.Carts.Any(y => !y.IsStatic && y.CreatedAt.Date >= dateFrom.Date && y.CreatedAt.Date <= dateTo.Date && y.State != State.Confirmed))
                .OrderBy(x => x.Name)
                .Select(x => new Client
                {
                    ID = x.ID,
                    Name = x.Name,
                    Address = x.Address,
                })
                .ToListAsync();
        }

        public async Task<List<CartsTransfersHistoryTable>> GetCartsTransfersHistoryTable(long clientID)
        {
            TransferRepository transferRepository = new(_db);
            var transfers = await transferRepository.GetLastTen(clientID);

            CartRepository cartRepository = new(_db);
            var carts = await cartRepository.GetLastTen(clientID);

            AbonoRepository abonoRepository = new(_db);
            var abonos = await abonoRepository.GetLastTen(clientID);
            
            var cartsTransfersHistory = new List<CartsTransfersHistoryTable>();

            foreach (var transfer in transfers)
            {
                cartsTransfersHistory.Add(new()
                {
                    Date = transfer.Date,
                    Type = CartsTransfersType.Transfer,
                    TransferAmount = transfer.Amount,
                });
            }

            foreach (var abono in abonos)
            {
                cartsTransfersHistory.Add(new()
                {
                    Date = abono.CreatedAt,
                    Type = CartsTransfersType.Abono,
                    AbonoName = abono.Abono.Name,
                    AbonoPrice = abono.Abono.Price,
                });
            }

            foreach (var cart in carts)
            {
                var payments = cart.PaymentMethods.ToList();
                var products = cart.Products.ToList();
               var abonoProducts = cart.AbonoProducts.ToList();

                cartsTransfersHistory.Add(new()
                {
                    Date = cart.CreatedAt,
                    Type = CartsTransfersType.Cart,
                    CartState = cart.State,
                    PaymentMethods = payments,
                    Products = products,
                    AbonoProducts = abonoProducts,
                });
            }
            return [.. cartsTransfersHistory.OrderByDescending(x => x.Date)];
        }

        public async Task<List<ClientAbono>> GetAbonos(long clientID)
        {
            return await _db
                .ClientAbonos
                .Where(x => x.ClientID == clientID)
                .ToListAsync();
        }

        public async Task<List<AbonoRenewalProduct>> GetAbonosRenewedAvailables(long clientID)
        {
            var today = DateTime.UtcNow.AddHours(-3);
            return await _db
                .AbonoRenewalProducts
                .Where(x =>
                    x.Type != ProductType.Máquina &&
                    x.AbonoRenewal.ClientID == clientID &&
                    x.CreatedAt.Month == today.Month &&
                    x.CreatedAt.Year == today.Year)
                .ToListAsync();
        }

        public async Task<List<Client>> GetUnassignedClients()
        {
            return await _db
                .Clients
                .Where(x => x.IsActive && string.IsNullOrEmpty(x.DealerID) && x.DeliveryDay == null)
                .ToListAsync();
        }
    }
}