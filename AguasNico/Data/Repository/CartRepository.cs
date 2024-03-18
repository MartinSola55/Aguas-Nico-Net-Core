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
    public class CartRepository(ApplicationDbContext db) : Repository<Cart>(db), ICartRepository
    {
        private readonly ApplicationDbContext _db = db;

        public async Task Update(Cart cart)
        {
            try
            {
                await _db.Database.BeginTransactionAsync();
                await SoftDelete(cart.ID);

                var client = await _db
                .Clients
                .Include(x => x.Products)
                    .ThenInclude(x => x.Product)
                .Where(x => x.ID == cart.ClientID)
                .FirstAsync() ?? throw new Exception("No se ha encontrado el cliente");

                if (cart.Products is not null)
                {
                    foreach(var product in cart.Products)
                    {
                        if (product.Quantity <= 0)
                            continue;

                        var clientProduct = client.Products.First(x => x.Product.Type == product.Type);
                        clientProduct.Stock += product.Quantity;
                        client.Debt += product.Quantity * clientProduct.Product.Price;

                        //Ignorar los filtros globales
                        var existingProduct = await _db
                            .CartProducts
                            .IgnoreQueryFilters()
                            .Where(x => x.CartID == cart.ID && x.Type == product.Type)
                            .FirstOrDefaultAsync();

                        if (existingProduct is not null)
                        {
                            existingProduct.Quantity = product.Quantity;
                            existingProduct.SettedPrice = clientProduct.Product.Price;
                            existingProduct.UpdatedAt = DateTime.UtcNow.AddHours(-3);
                            existingProduct.DeletedAt = null;
                        }
                        else
                        {
                            await _db.CartProducts.AddAsync(new()
                            {
                                CartID = cart.ID,
                                Type = product.Type,
                                Quantity = product.Quantity,
                                SettedPrice = clientProduct.Product.Price,
                            });
                        }
                    }
                }

                if (cart.AbonoProducts is not null)
                {
                    foreach (var product in cart.AbonoProducts)
                    {
                        if (product.Quantity <= 0)
                            continue;

                        var clientProduct = client.Products.First(x => x.Product.Type == product.Type);
                        clientProduct.Stock += product.Quantity;

                    var abonoProducts = await _db
                        .AbonoRenewalProducts
                        .Where(x =>
                            x.AbonoRenewal.ClientID == cart.ClientID &&
                            x.CreatedAt.Month == cart.CreatedAt.Month &&
                            x.CreatedAt.Year == cart.CreatedAt.Year &&
                            x.Type == product.Type)
                        .ToListAsync();

                        if (abonoProducts.Sum(x => x.Available) < product.Quantity)
                            throw new Exception("El cliente no posee suficientes " + product.Type.GetDisplayName() + " disponibles en sus abonos");

                        int quantity = product.Quantity;
                        foreach (var abonoProduct in abonoProducts)
                        {
                            if (abonoProduct.Available >= quantity)
                            {
                                abonoProduct.Available -= quantity;
                                break;
                            }
                            else
                            {
                                quantity -= abonoProduct.Available;
                                abonoProduct.Available = 0;
                            }
                        }

                        //Ignorar los filtros globales
                        var existingProduct = await _db
                            .CartAbonoProducts
                            .IgnoreQueryFilters()
                            .Where(x => x.CartID == cart.ID && x.Type == product.Type)
                            .FirstOrDefaultAsync();

                        if (existingProduct is not null)
                        {
                            existingProduct.Quantity = product.Quantity;
                            existingProduct.UpdatedAt = DateTime.UtcNow.AddHours(-3);
                            existingProduct.DeletedAt = null;
                        }
                        else
                        {
                            await _db.CartAbonoProducts.AddAsync(new()
                            {
                                CartID = cart.ID,
                                Type = product.Type,
                                Quantity = product.Quantity,
                            });
                        }
                    }
                }

                if (cart.ReturnedProducts is not null)
                {
                    foreach (var product in cart.ReturnedProducts)
                    {
                        var clientProduct = client.Products.First(x => x.Product.Type == product.Type);

                        if (clientProduct.Stock < product.Quantity)
                            throw new Exception("El cliente no posee stock suficiente de: " + product.Type.GetDisplayName());

                        clientProduct.Stock -= product.Quantity;

                        //Ignorar los filtros globales
                        var existingProduct = await _db
                            .ReturnedProducts
                            .IgnoreQueryFilters()
                            .Where(x => x.CartID == cart.ID && x.Type == product.Type)
                            .FirstOrDefaultAsync();

                        if (existingProduct is not null)
                        {
                            existingProduct.Quantity = product.Quantity;
                            existingProduct.UpdatedAt = DateTime.UtcNow.AddHours(-3);
                            existingProduct.DeletedAt = null;
                        }
                        else
                        {
                            await _db.ReturnedProducts.AddAsync(new()
                            {
                                CartID = cart.ID,
                                Type = product.Type,
                                Quantity = product.Quantity,
                            });
                        }
                    }
                }

                if (cart.PaymentMethods is not null)
                {
                    foreach (var paymentMethod in cart.PaymentMethods)
                    {
                        client.Debt -= paymentMethod.Amount;

                        //Ignorar los filtros globales
                        var existingMethod = await _db
                            .CartPaymentMethods
                            .IgnoreQueryFilters()
                            .Where(x => x.CartID == cart.ID && x.PaymentMethodID == paymentMethod.PaymentMethodID)
                            .FirstOrDefaultAsync();

                        if (existingMethod is not null)
                        {
                            existingMethod.Amount = paymentMethod.Amount;
                            existingMethod.UpdatedAt = DateTime.UtcNow.AddHours(-3);
                            existingMethod.DeletedAt = null;
                        }
                        else
                        {
                            await _db.CartPaymentMethods.AddAsync(new()
                            {
                                CartID = cart.ID,
                                PaymentMethodID = paymentMethod.PaymentMethodID,
                                Amount = paymentMethod.Amount,
                            });
                        }
                    }
                }

                var updatedCart = await _db
                    .Carts
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(x => x.ID == cart.ID) ?? throw new Exception("No se ha encontrado la bajada");

                updatedCart.DeletedAt = null;
                updatedCart.UpdatedAt = DateTime.UtcNow.AddHours(-3);

                await _db.SaveChangesAsync();
                await _db.Database.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _db.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task SoftDelete(long id)
        {
            var cart = await _db
                .Carts
                .Include(x => x.Client)
                .Include(x => x.Products)
                .Include(x => x.AbonoProducts)
                .Include(x => x.ReturnedProducts)
                .Include(x => x.PaymentMethods)
                .FirstOrDefaultAsync(x => x.ID == id) ?? throw new Exception("No se ha encontrado la bajada");

            // Productos de la bajada
            if (cart.Products is not null)
            {
                foreach (var product in cart.Products)
                {
                    var clientProduct = await _db
                        .ClientProducts
                        .Where(x => x.ClientID == cart.ClientID && x.Product.Type == product.Type)
                        .FirstOrDefaultAsync() ?? throw new Exception("No se ha encontrado un producto del cliente");

                    clientProduct.Stock -= product.Quantity;
                    cart.Client.Debt -= product.Quantity * product.SettedPrice;
                    product.DeletedAt = DateTime.UtcNow.AddHours(-3);
                }
            }

            // Productos de abonos de la bajada

            if (cart.AbonoProducts is not null)
            {
                foreach (var abonoProductInCart in cart.AbonoProducts)
                {
                    var abonoProduct = await _db
                        .AbonoRenewalProducts
                        .Where(x =>
                            x.AbonoRenewal.ClientID == cart.ClientID &&
                            x.CreatedAt.Month == cart.CreatedAt.Month &&
                            x.CreatedAt.Year == cart.CreatedAt.Year &&
                            x.Type == abonoProductInCart.Type)
                        .FirstOrDefaultAsync() ?? throw new Exception("No se ha encontrado un producto del abono del cliente");

                    abonoProduct.Available += abonoProductInCart.Quantity;

                    var clientProduct = await _db
                        .ClientProducts
                        .Where(x => x.ClientID == cart.ClientID && x.Product.Type == abonoProductInCart.Type)
                        .FirstOrDefaultAsync() ?? throw new Exception("No se ha encontrado un producto del cliente");

                    clientProduct.Stock -= abonoProductInCart.Quantity;
                    abonoProductInCart.DeletedAt = DateTime.UtcNow.AddHours(-3);
                }
            }

            // Productos devueltos de la bajada
            if (cart.ReturnedProducts is not null)
            {
                foreach (var product in cart.ReturnedProducts)
                {
                    var clientProduct = await _db
                        .ClientProducts
                        .Where(x => x.ClientID == cart.ClientID && x.Product.Type == product.Type)
                        .FirstOrDefaultAsync() ?? throw new Exception("No se ha encontrado un producto del cliente");

                    clientProduct.Stock += product.Quantity;
                    product.DeletedAt = DateTime.UtcNow.AddHours(-3);
                }
            }

            // Metodos de pago de la bajada
            if (cart.PaymentMethods is not null)
            {
                foreach (var paymentMethod in cart.PaymentMethods)
                {
                    cart.Client.Debt += paymentMethod.Amount;
                    paymentMethod.DeletedAt = DateTime.UtcNow.AddHours(-3);
                }
            }

            cart.DeletedAt = DateTime.UtcNow.AddHours(-3);
            try
            {
                bool isInTransaction = _db.Database.CurrentTransaction is not null;

                if (!isInTransaction) await _db.Database.BeginTransactionAsync();
                
                await _db.SaveChangesAsync();

                if (!isInTransaction) await _db.Database.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _db.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<Cart>> GetLastTen(long clientID)
        {
            return await _db.Carts
                .Where(x => x.ClientID == clientID && !x.IsStatic && x.State != State.Pending)
                .OrderByDescending(x => x.CreatedAt)
                .Take(10)
                .Include(x => x.Products)
                .Include(x => x.AbonoProducts)
                .Include(x => x.PaymentMethods)
                    .ThenInclude(x => x.PaymentMethod)
                .ToListAsync();
        }

        public async Task Confirm(Cart cart)
        {
            var today = DateTime.UtcNow.AddHours(-3);
            var client = await _db
                .Clients
                .FindAsync(cart.ClientID) ?? throw new Exception("No se ha encontrado el cliente");

            var returnedProducts = new List<ReturnedProduct>();
            foreach (var type in new ConstantsMethods().GetProductTypes())
            {
                returnedProducts.Add(new()
                {
                    CartID = cart.ID,
                    Type = type,
                    Quantity = 0,
                });
            }

            decimal total = 0;
            if (cart.Products is not null)
            {
                foreach (var product in cart.Products)
                {
                    if (product.Quantity <= 0)
                            continue;
                    var clientProduct = await _db
                        .ClientProducts
                        .Where(x => x.ClientID == cart.ClientID && x.Product.Type == product.Type)
                        .Include(x => x.Product)
                        .FirstOrDefaultAsync() ?? throw new Exception("No se ha encontrado un producto del cliente");

                    clientProduct.Stock += product.Quantity;
                    product.SettedPrice = clientProduct.Product.Price;
                    product.CartID = cart.ID;
                    total += product.Quantity * product.SettedPrice;
                    
                    await _db.CartProducts.AddAsync(product);

                    if (returnedProducts.Any(x => x.Type == product.Type))
                    {
                        var returnedProduct = returnedProducts.First(x => x.Type == product.Type);
                        returnedProduct.Quantity += product.Quantity;
                    }
                }
                client.Debt += total;
            }

            if (cart.AbonoProducts is not null)
            {
                foreach (var abonoProduct in cart.AbonoProducts)
                {
                    if (abonoProduct.Quantity <= 0) continue;

                    var abonoProductsList = await _db
                        .AbonoRenewalProducts
                        .Where(x =>
                            x.AbonoRenewal.ClientID == cart.ClientID &&
                            x.CreatedAt.Month == today.Month &&
                            x.CreatedAt.Year == today.Year &&
                            x.Type == abonoProduct.Type)
                        .ToListAsync();

                    if (abonoProductsList.Count == 0)
                            throw new Exception("No se ha encontrado un producto del abono del cliente");

                    if (abonoProductsList.Sum(x => x.Available) < abonoProduct.Quantity)
                            throw new Exception("El cliente no posee stock suficiente de: " + abonoProduct.Type.GetDisplayName());
                        
                    int quantity = abonoProduct.Quantity;
                    foreach (var abonoProductList in abonoProductsList)
                    {
                        if (abonoProductList.Available >= quantity)
                        {
                            abonoProductList.Available -= quantity;
                            break;
                        }
                        else
                        {
                            quantity -= abonoProductList.Available;
                            abonoProductList.Available = 0;
                        }
                    }

                    var clientProduct = await _db
                        .ClientProducts
                        .Where(x => x.ClientID == cart.ClientID && x.Product.Type == abonoProduct.Type)
                        .Include(x => x.Product)
                        .FirstOrDefaultAsync() ?? throw new Exception("No se ha encontrado un producto del cliente");
                    clientProduct.Stock += abonoProduct.Quantity;

                    abonoProduct.CartID = cart.ID;
                    await _db.CartAbonoProducts.AddAsync(abonoProduct);

                    if (returnedProducts.Any(x => x.Type == abonoProduct.Type))
                    {
                        var returnedProduct = returnedProducts.First(x => x.Type == abonoProduct.Type);
                        returnedProduct.Quantity += abonoProduct.Quantity;
                    }
                }
            }

            total = 0;
            if (cart.PaymentMethods is not null)
            {
                foreach (var paymentMethod in cart.PaymentMethods)
                {
                    paymentMethod.CartID = cart.ID;
                    total += paymentMethod.Amount;
                    await _db.CartPaymentMethods.AddAsync(paymentMethod);
                }
                client.Debt -= total;
            }

            foreach (var product in returnedProducts)
            {
                if (product.Quantity <= 0) continue;
                var clientProduct = await _db
                    .ClientProducts
                    .Where(x => x.ClientID == cart.ClientID && x.Product.Type == product.Type)
                    .FirstOrDefaultAsync() ?? throw new Exception("No se ha encontrado un producto del cliente");

                clientProduct.Stock -= product.Quantity;
                await _db.ReturnedProducts.AddAsync(product);
            }

            var cartToUpdate = await _db
                .Carts
                .FindAsync(cart.ID) ?? throw new Exception("No se ha encontrado la bajada");

            cartToUpdate.State = State.Confirmed;
            cartToUpdate.UpdatedAt = DateTime.UtcNow.AddHours(-3);

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

        public async Task CreateManual(Cart cart)
        {
            cart.State = State.Confirmed;
            cart.IsStatic = false;
            cart.Priority = await _db
                .Carts
                .Where(x => x.RouteID == cart.RouteID)
                .MaxAsync(x => x.Priority) + 1;

            await _db.Carts.AddAsync(cart);

            var today = DateTime.UtcNow.AddHours(-3);
            var client = await _db
                .Clients
                .FindAsync(cart.ClientID) ?? throw new Exception("No se ha encontrado el cliente");

            var returnedProducts = new List<ReturnedProduct>();
            foreach (ProductType type in new ConstantsMethods().GetProductTypes())
            {
                returnedProducts.Add(new()
                {
                    Cart = cart,
                    Type = type,
                    Quantity = 0,
                });
            }

            decimal total = 0;
            if (cart.Products is not null)
            {
                foreach (var product in cart.Products)
                {
                    if (product.Quantity <= 0)
                            continue;

                    var clientProduct = await _db
                        .ClientProducts
                        .Where(x => x.ClientID == cart.ClientID && x.Product.Type == product.Type)
                        .Include(x => x.Product)
                        .FirstOrDefaultAsync() ?? throw new Exception("No se ha encontrado un producto del cliente");

                    clientProduct.Stock += product.Quantity;
                    product.SettedPrice = clientProduct.Product.Price;
                    total += product.Quantity * product.SettedPrice;

                    if (returnedProducts.Any(x => x.Type == product.Type))
                    {
                        var returnedProduct = returnedProducts.First(x => x.Type == product.Type);
                        returnedProduct.Quantity += product.Quantity;
                    }
                }
                client.Debt += total;
            }

            if (cart.AbonoProducts is not null)
            {
                foreach (var abonoProduct in cart.AbonoProducts)
                {
                    if (abonoProduct.Quantity <= 0)
                        continue;

                    var abonoProductsList = await _db
                        .AbonoRenewalProducts
                        .Where(x =>
                            x.AbonoRenewal.ClientID == cart.ClientID &&
                            x.CreatedAt.Month == today.Month &&
                            x.CreatedAt.Year == today.Year &&
                            x.Type == abonoProduct.Type)
                        .ToListAsync();

                    if (abonoProductsList.Count == 0)
                        throw new Exception("No se ha encontrado un producto del abono del cliente");

                    if (abonoProductsList.Sum(x => x.Available) < abonoProduct.Quantity)
                        throw new Exception("El cliente no posee stock suficiente de: " + abonoProduct.Type.GetDisplayName());

                    int quantity = abonoProduct.Quantity;
                    foreach (var abonoProductList in abonoProductsList)
                    {
                        if (abonoProductList.Available >= quantity)
                        {
                            abonoProductList.Available -= quantity;
                            break;
                        }
                        else
                        {
                            quantity -= abonoProductList.Available;
                            abonoProductList.Available = 0;
                        }
                    }

                    var clientProduct = await _db
                        .ClientProducts
                        .Where(x => x.ClientID == cart.ClientID && x.Product.Type == abonoProduct.Type)
                        .Include(x => x.Product)
                        .FirstOrDefaultAsync() ?? throw new Exception("No se ha encontrado un producto del cliente");

                    clientProduct.Stock += abonoProduct.Quantity;

                    if (returnedProducts.Any(x => x.Type == abonoProduct.Type))
                    {
                        var returnedProduct = returnedProducts.First(x => x.Type == abonoProduct.Type);
                        returnedProduct.Quantity += abonoProduct.Quantity;
                    }
                }
            }

            total = 0;
            if (cart.PaymentMethods is not null)
            {
                foreach (var paymentMethod in cart.PaymentMethods)
                {
                    paymentMethod.Cart = cart;
                    total += paymentMethod.Amount;
                }
                client.Debt -= total;
            }

            foreach (var product in returnedProducts)
            {
                if (product.Quantity <= 0)
                    continue;

                var clientProduct = await _db
                    .ClientProducts
                    .Where(x => x.ClientID == cart.ClientID && x.Product.Type == product.Type)
                    .FirstOrDefaultAsync() ?? throw new Exception("No se ha encontrado un producto del cliente");

                clientProduct.Stock -= product.Quantity;
                await _db.ReturnedProducts.AddAsync(product);
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

        public async Task<List<ReturnedProduct>> GetReturnedProducts(long cartID)
        {
            var cart = await _db
                .Carts
                .FindAsync(cartID) ?? throw new Exception("No se ha encontrado la bajada");

            var returnedProducts = await _db
                .ReturnedProducts
                .Where(x => x.CartID == cartID)
                .ToListAsync();

            var products = new List<ReturnedProduct>();
            var clientProducts = await _db
                .ClientProducts
                .Where(x => x.ClientID == cart.ClientID)
                .Include(x => x.Product)
                .ToListAsync();

            foreach (var product in clientProducts)
            {
                if (returnedProducts.Any(x => x.Type == product.Product.Type))
                {
                    products.Add(returnedProducts.First(x => x.Type == product.Product.Type));
                }
                else
                {
                    products.Add(new()
                    {
                        Type = product.Product.Type,
                        Quantity = 0,
                    });
                }
            }
            return products;
        }

        public async Task ReturnProducts(long cartID, List<ReturnedProduct> products)
        {
            var cart = await _db
                .Carts
                .Include(x => x.ReturnedProducts)
                .Where(x => x.ID == cartID)
                .FirstAsync() ?? throw new Exception("No se ha encontrado la bajada");

            var client = await _db
                .Clients
                .Include(x => x.Products)
                    .ThenInclude(x => x.Product)
                .Where(x => x.ID == cart.ClientID)
                .FirstAsync() ?? throw new Exception("No se ha encontrado el cliente");
                
            foreach (var product in cart.ReturnedProducts)
            {
                var clientProduct = client.Products.First(x => x.Product.Type == product.Type);
                clientProduct.Stock += product.Quantity;
                product.DeletedAt = DateTime.UtcNow.AddHours(-3);
            }

            foreach (var product in products)
            {
                if (product.Quantity <= 0)
                    continue;

                var clientProduct = client.Products.First(x => x.Product.Type == product.Type);

                if (clientProduct.Stock < product.Quantity)
                    throw new Exception("El cliente no posee stock suficiente de: " + product.Type.GetDisplayName());

                clientProduct.Stock -= product.Quantity;

                //Ignorar los filtros globales
                var existingReturnedProducts = await _db
                    .ReturnedProducts
                    .IgnoreQueryFilters()
                    .Where(x => x.CartID == cart.ID && x.Type == product.Type)
                    .FirstOrDefaultAsync();

                if (existingReturnedProducts is not null)
                {
                    existingReturnedProducts.Quantity = product.Quantity;
                    existingReturnedProducts.UpdatedAt = DateTime.UtcNow.AddHours(-3);
                    existingReturnedProducts.DeletedAt = null;
                }
                else
                {
                    await _db.ReturnedProducts.AddAsync(new()
                    {
                        Cart = cart,
                        Type = clientProduct.Product.Type,
                        Quantity = product.Quantity,
                    });
                }
            }
                
            cart.UpdatedAt = DateTime.UtcNow.AddHours(-3);

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
    }
}