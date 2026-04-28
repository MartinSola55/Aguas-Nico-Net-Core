using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using Microsoft.AspNetCore.Mvc;
using AguasNico.Models.ViewModels.Tables;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AguasNico.Data.Repository
{
    public class TablesRepository(ApplicationDbContext db) : ITablesRepository
    {
        private readonly ApplicationDbContext _db = db;

        public async Task<List<InvoiceTable>> GetInvoicesByDates(DateTime startDate, DateTime endDate, Day? invoiceDay, string invoiceDealer)
        {
            var clients = await GetInvoiceClientsQuery(invoiceDay, invoiceDealer)
                .OrderBy(x => x.Name)
                .ToListAsync();

            var clientIDs = clients.Select(x => x.ID).ToList();

            var cartProducts = await _db
                .CartProducts
                .Include(x => x.Cart)
                .Where(x =>
                    clientIDs.Contains(x.Cart.ClientID) &&
                    x.CreatedAt.Date >= startDate.Date &&
                    x.CreatedAt.Date <= endDate.Date &&
                    x.SettedPrice > 0)
                .ToListAsync();

            // Do not show abono products
            //var cartAbonoProducts = await _db
            //    .CartAbonoProducts
            //    .Include(x => x.Cart)
            //    .Where(x => clientIDs.Contains(x.Cart.ClientID) && x.CreatedAt.Date >= startDate.Date && x.CreatedAt.Date <= endDate.Date)
            //    .ToListAsync();

            List<InvoiceTable> invoices = [];
            foreach (var client in clients)
            {
                var cartProductsByClient = cartProducts.Where(x => x.Cart.ClientID == client.ID).ToList();
                if (cartProductsByClient.Count > 0)
                    invoices.Add(new()
                    {
                        Client = client,
                        Products = [.. cartProductsByClient.GroupBy(x => x.Type).Select(Type => new InvoiceProduct
                        {
                            Type = Type.Key.GetDisplayName(),
                            Quantity = Type.Sum(x => x.Quantity),
                            Total = Type.Sum(x => x.SettedPrice * x.Quantity),
                        })],
                    });

                //var abonoProductsByClient = cartAbonoProducts.Where(x => x.Cart.ClientID == client.ID).ToList();
                //if (abonoProductsByClient.Count > 0)
                //{
                //    var invoice = invoices.FirstOrDefault(x => x.Client.ID == client.ID);
                //    if (invoice != null)
                //    {
                //        foreach (var abonoProduct in abonoProductsByClient)
                //        {
                //            var product = invoice.Products.FirstOrDefault(x => x.Type == abonoProduct.Type.GetDisplayName());
                //            if (product != null)
                //            {
                //                product.Quantity += abonoProduct.Quantity;
                //            }
                //            else
                //            {
                //                invoice.Products.Add(new InvoiceProduct
                //                {
                //                    Type = abonoProduct.Type.GetDisplayName(),
                //                    Quantity = abonoProduct.Quantity,
                //                    Total = 0,
                //                });
                //            }
                //        }
                //    }
                //    else
                //    {
                //        invoices.Add(new()
                //        {
                //            Client = client,
                //            Products = abonoProductsByClient.GroupBy(x => x.Type).Select(Type => new InvoiceProduct
                //            {
                //                Type = Type.Key.GetDisplayName(),
                //                Quantity = Type.Sum(x => x.Quantity),
                //                Total = 0,
                //            }).ToList(),
                //        });
                //    }
                //}
            }

            return invoices;
        }

        public async Task<List<InvoiceCsvRow>> GetInvoicesCsvData(DateTime startDate, DateTime endDate, Day? invoiceDay, string invoiceDealer)
        {
            var clients = await GetInvoiceClientsQuery(invoiceDay, invoiceDealer)
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.Address,
                    x.Email,
                    x.CUIT,
                    x.InvoiceType,
                    x.TaxCondition,
                })
                .ToListAsync();

            var clientIds = clients.Select(x => x.ID).ToList();

            var cartProducts = await _db.CartProducts
                .Where(x =>
                    clientIds.Contains(x.Cart.ClientID) &&
                    x.CreatedAt.Date >= startDate.Date &&
                    x.CreatedAt.Date <= endDate.Date &&
                    x.SettedPrice > 0)
                .Select(x => new { x.Cart.ClientID, x.Type, x.Quantity, x.SettedPrice })
                .ToListAsync();

            var cartAbonoProducts = await _db.CartAbonoProducts
                .Where(x =>
                    clientIds.Contains(x.Cart.ClientID) &&
                    x.CreatedAt.Date >= startDate.Date &&
                    x.CreatedAt.Date <= endDate.Date)
                .Select(x => new { x.Cart.ClientID, x.Type, x.Quantity })
                .ToListAsync();

            var result = new List<InvoiceCsvRow>();
            foreach (var client in clients)
            {
                var products = cartProducts
                    .Where(x => x.ClientID == client.ID)
                    .GroupBy(x => x.Type)
                    .Select(g => new InvoiceProductCsv
                    {
                        Type = g.Key.GetDisplayName(),
                        Quantity = g.Sum(x => x.Quantity),
                        Subtotal = g.Sum(x => x.SettedPrice * x.Quantity),
                    })
                    .ToList();

                if (products.Count == 0)
                    continue;

                foreach (var abonoGroup in cartAbonoProducts.Where(x => x.ClientID == client.ID).GroupBy(x => x.Type))
                {
                    var existing = products.FirstOrDefault(x => x.Type == abonoGroup.Key.GetDisplayName());
                    if (existing != null)
                        existing.Quantity += abonoGroup.Sum(x => x.Quantity);
                }

                decimal total = products.Sum(x => x.Subtotal);
                result.Add(new InvoiceCsvRow
                {
                    ExternalId = $"SLN-{client.ID}-{DateTime.Now:yyyyMMddHHmmss}",
                    ClientCuit = client.CUIT ?? "",
                    InvoiceTypeId = GetInvoiceTypeCode(client.InvoiceType),
                    Neto = total / 1.21m,
                    IvaRate = 21,
                    Total = total,
                    TaxConditionTypeId = (int)client.TaxCondition.GetValueOrDefault(),
                    ClientName = client.Name,
                    ClientAddress = client.Address,
                    Description = FormatCsvDescription(products),
                    Email = client.Email ?? "",
                });
            }

            return result;
        }

        private IQueryable<Client> GetInvoiceClientsQuery(Day? invoiceDay, string invoiceDealer)
        {
            var query = _db.Clients.Where(x =>
                x.DealerID == invoiceDealer &&
                x.IsActive &&
                x.HasInvoice &&
                x.InvoiceType.HasValue &&
                x.TaxCondition.HasValue &&
                !string.IsNullOrEmpty(x.CUIT));

            if (invoiceDay.HasValue)
                query = query.Where(x => x.DeliveryDay == invoiceDay.Value);

            return query;
        }

        private static string GetInvoiceTypeCode(InvoiceType? invoiceType) => invoiceType switch
        {
            InvoiceType.A => "1",
            InvoiceType.B => "6",
            //InvoiceType.C => "11",
            _ => "",
        };

        private static string FormatCsvDescription(List<InvoiceProductCsv> products)
        {
            return string.Join(",", products.Select(p => $"[{p.Type}, {p.Quantity}, {Constants.INVOICE_UNIT_TYPE}, {(int)p.Subtotal}]"));
        }

        public async Task<List<SoldProductsTable>> GetSoldProductsByDate(DateTime date)
        {
            var cartProducts = await _db
                .CartProducts
                .Where(x => x.Cart.CreatedAt.Date == date.Date)
                .ToListAsync();

            var cartAbonoProducts = await _db
                .CartAbonoProducts
                .Where(x => x.Cart.CreatedAt.Date == date.Date)
                .ToListAsync();

            var dispatchedProducts = await _db
                .DispatchedProducts
                .Where(x => x.Route.CreatedAt.Date == date.Date)
                .ToListAsync();

            var returnedProducts = await _db
                .ReturnedProducts
                .Where(x => x.Cart.CreatedAt.Date == date.Date)
                .ToListAsync();

            var soldProducts = new List<SoldProductsTable>();

            foreach (ProductType type in Enum.GetValues(typeof(ProductType)))
            {
                var cartProductsByType = cartProducts.Where(x => x.Type == type).ToList();
                var cartAbonoProductsByType = cartAbonoProducts.Where(x => x.Type == type).ToList();
                var dispatchedProductsByType = dispatchedProducts.Where(x => x.Type == type).ToList();
                var returnedProductsByType = returnedProducts.Where(x => x.Type == type).ToList();

                var soldProduct = new SoldProductsTable()
                {
                    Name = type.GetDisplayName(),
                    Sold = cartProductsByType != null ? cartProductsByType.Sum(x => x.Quantity) : 0,
                    Dispatched = dispatchedProductsByType != null ? dispatchedProductsByType.Sum(x => x.Quantity) : 0,
                    Returned = returnedProductsByType != null ? returnedProductsByType.Sum(x => x.Quantity) : 0,
                };
                soldProduct.Sold += cartAbonoProductsByType != null ? cartAbonoProductsByType.Sum(x => x.Quantity) : 0;

                soldProducts.Add(soldProduct);
            }

            return soldProducts;
        }

        public async Task<List<SoldProductsTable>> GetSoldProductsByDateAndRoute(DateTime date, long routeID)
        {
            var cartProducts = await _db
                .CartProducts
                .Where(x => x.CreatedAt.Date == date.Date && x.Cart.RouteID == routeID)
                .ToListAsync();

            var cartAbonoProducts = await _db
                .CartAbonoProducts
                .Where(x => x.CreatedAt.Date == date.Date && x.Cart.RouteID == routeID)
                .ToListAsync();

            var dispatchedProducts = await _db
                .DispatchedProducts
                .Where(x => x.CreatedAt.Date == date.Date && x.RouteID == routeID)
                .ToListAsync();

            var returnedProducts = await _db
                .ReturnedProducts
                .Where(x => x.CreatedAt.Date == date.Date && x.Cart.RouteID == routeID)
                .ToListAsync();

            var soldProducts = new List<SoldProductsTable>();

            foreach (ProductType type in Enum.GetValues(typeof(ProductType)))
            {
                var cartProductsByType = cartProducts.Where(x => x.Type == type).ToList();
                var cartAbonoProductsByType = cartAbonoProducts.Where(x => x.Type == type).ToList();
                var dispatchedProductsByType = dispatchedProducts.Where(x => x.Type == type).ToList();
                var returnedProductsByType = returnedProducts.Where(x => x.Type == type).ToList();

                var soldProduct = new SoldProductsTable()
                {
                    Name = type.GetDisplayName(),
                    Sold = cartProductsByType != null ? cartProductsByType.Sum(x => x.Quantity) : 0,
                    Dispatched = dispatchedProductsByType != null ? dispatchedProductsByType.Sum(x => x.Quantity) : 0,
                    Returned = returnedProductsByType != null ? returnedProductsByType.Sum(x => x.Quantity) : 0,
                };
                soldProduct.Sold += cartAbonoProductsByType != null ? cartAbonoProductsByType.Sum(x => x.Quantity) : 0;

                soldProducts.Add(soldProduct);
            }

            return soldProducts;
        }

        public async Task<List<SoldProductsTable>> GetSoldProductsByRoute(long routeID)
        {
            var cartProducts = await _db
                .CartProducts
                .Where(x => x.Cart.RouteID == routeID)
                .ToListAsync();

            var cartAbonoProducts = await _db
                .CartAbonoProducts
                .Where(x => x.Cart.RouteID == routeID)
                .ToListAsync();

            var dispatchedProducts = await _db
                .DispatchedProducts
                .Where(x => x.RouteID == routeID)
                .ToListAsync();

            var returnedProducts = await _db
                .ReturnedProducts
                .Where(x => x.Cart.RouteID == routeID)
                .ToListAsync();

            var clientStock = await _db
                .Carts
                .Where(x => x.RouteID == routeID)
                .Select(x => x.Client)
                .SelectMany(x => x.Products)
                .Include(x => x.Product)
                .ToListAsync();

            var soldProducts = new List<SoldProductsTable>();

            foreach (ProductType type in Enum.GetValues(typeof(ProductType)))
            {
                var cartProductsByType = cartProducts.Where(x => x.Type == type).ToList();
                var cartAbonoProductsByType = cartAbonoProducts.Where(x => x.Type == type).ToList();
                var dispatchedProductsByType = dispatchedProducts.Where(x => x.Type == type).ToList();
                var returnedProductsByType = returnedProducts.Where(x => x.Type == type).ToList();
                var clientStockByType = clientStock.Where(x => x.Product.Type == type).ToList();

                var soldProduct = new SoldProductsTable()
                {
                    Name = type.GetDisplayName(),
                    Sold = cartProductsByType != null ? cartProductsByType.Sum(x => x.Quantity) : 0,
                    Dispatched = dispatchedProductsByType != null ? dispatchedProductsByType.Sum(x => x.Quantity) : 0,
                    Returned = returnedProductsByType != null ? returnedProductsByType.Sum(x => x.Quantity) : 0,
                    ClientStock = clientStockByType != null ? clientStockByType.Sum(x => x.Stock) : 0,
                };

                soldProduct.Sold += cartAbonoProductsByType != null ? cartAbonoProductsByType.Sum(x => x.Quantity) : 0;

                soldProducts.Add(soldProduct);
            }
            return soldProducts;
        }

        public async Task<List<SoldProductsTable>> GetSoldProductsBetweenDates(DateTime dateFrom, DateTime dateTo, string dealerID)
        {
            var cartProducts = await _db
                .CartProducts
                .Where(x => x.Cart.CreatedAt.Date >= dateFrom.Date && x.Cart.CreatedAt.Date <= dateTo.Date && x.Cart.Route.UserID == dealerID)
                .ToListAsync();

            var cartAbonoProducts = await _db
                .CartAbonoProducts
                .Where(x => x.Cart.CreatedAt.Date >= dateFrom.Date && x.Cart.CreatedAt.Date <= dateTo.Date && x.Cart.Route.UserID == dealerID)
                .ToListAsync();

            var soldProducts = new List<SoldProductsTable>();

            foreach (ProductType type in Enum.GetValues(typeof(ProductType)))
            {
                var cartProductsByType = cartProducts.Where(x => x.Type == type).ToList();
                var cartAbonoProductsByType = cartAbonoProducts.Where(x => x.Type == type).ToList();

                var soldProduct = new SoldProductsTable()
                {
                    Name = type.GetDisplayName(),
                    Sold = cartProductsByType != null ? cartProductsByType.Sum(x => x.Quantity) : 0,
                    Total = cartProductsByType != null ? cartProductsByType.Sum(x => x.SettedPrice * x.Quantity) : 0,
                };
                soldProduct.Sold += cartAbonoProductsByType != null ? cartAbonoProductsByType.Sum(x => x.Quantity) : 0;

                soldProducts.Add(soldProduct);
            }

            return soldProducts;
        }
    }
}
