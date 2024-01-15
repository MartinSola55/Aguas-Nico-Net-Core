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

        public List<SoldProductsTable> GetSoldProductsByDate(DateTime date)
        {
            List<CartProduct> cartProducts = [.. _db.CartProducts.Where(x => x.CreatedAt.Date == date.Date)];
            List<DispatchedProduct> dispatchedProducts = [.. _db.DispatchedProducts.Where(x => x.CreatedAt.Date == date.Date)];
            List<ReturnedProduct> returnedProducts = [.. _db.ReturnedProducts.Where(x => x.CreatedAt.Date == date.Date)];
            List<SoldProductsTable> soldProducts = [];

            foreach (ProductType type in Enum.GetValues(typeof(ProductType)))
            {
                List<CartProduct> cartProductsByType = cartProducts.Where(x => x.Type == type).ToList();
                List<DispatchedProduct> dispatchedProductsByType = dispatchedProducts.Where(x => x.Type == type).ToList();
                List<ReturnedProduct> returnedProductsByType = returnedProducts.Where(x => x.Type == type).ToList();
                soldProducts.Add(new SoldProductsTable
                {
                    Name = type.GetDisplayName(),
                    Sold = cartProductsByType != null ? cartProductsByType.Sum(x => x.Quantity) : 0,
                    Dispatched = dispatchedProductsByType != null ? dispatchedProductsByType.Sum(x => x.Quantity) : 0,
                    Returned = returnedProductsByType != null ? returnedProductsByType.Sum(x => x.Quantity) : 0,
                });
            }

            return soldProducts;
        }

        public List<SoldProductsTable> GetSoldProductsByDateAndRoute(DateTime date, long routeID)
        {
            List<CartProduct> cartProducts = [.. _db.CartProducts.Where(x => x.CreatedAt.Date == date.Date && x.Cart.RouteID == routeID)];
            List<DispatchedProduct> dispatchedProducts = [.. _db.DispatchedProducts.Where(x => x.CreatedAt.Date == date.Date && x.RouteID == routeID)];
            List<ReturnedProduct> returnedProducts = [.. _db.ReturnedProducts.Where(x => x.CreatedAt.Date == date.Date && x.Cart.RouteID == routeID)];
            List<SoldProductsTable> soldProducts = [];

            foreach (ProductType type in Enum.GetValues(typeof(ProductType)))
            {
                List<CartProduct> cartProductsByType = cartProducts.Where(x => x.Type == type).ToList();
                List<DispatchedProduct> dispatchedProductsByType = dispatchedProducts.Where(x => x.Type == type).ToList();
                List<ReturnedProduct> returnedProductsByType = returnedProducts.Where(x => x.Type == type).ToList();
                soldProducts.Add(new SoldProductsTable
                {
                    Name = type.GetDisplayName(),
                    Sold = cartProductsByType != null ? cartProductsByType.Sum(x => x.Quantity) : 0,
                    Dispatched = dispatchedProductsByType != null ? dispatchedProductsByType.Sum(x => x.Quantity) : 0,
                    Returned = returnedProductsByType != null ? returnedProductsByType.Sum(x => x.Quantity) : 0,
                });
            }

            return soldProducts;
        }

        public List<SoldProductsTable> GetSoldProductsByRoute(long routeID)
        {
            try
            {
                List<CartProduct> cartProducts = [.. _db.CartProducts.Where(x => x.Cart.RouteID == routeID)];
                List<DispatchedProduct> dispatchedProducts = [.. _db.DispatchedProducts.Where(x => x.RouteID == routeID)];
                List<ReturnedProduct> returnedProducts = [.. _db.ReturnedProducts.Where(x => x.Cart.RouteID == routeID)];
                List<ClientProduct> clientStock = [.. _db.Carts.Where(x => x.RouteID == routeID).Select(x => x.Client).SelectMany(x => x.Products).Include(x => x.Product)];
                List<SoldProductsTable> soldProducts = [];

                foreach (ProductType type in Enum.GetValues(typeof(ProductType)))
                {
                    List<CartProduct> cartProductsByType = cartProducts.Where(x => x.Type == type).ToList();
                    List<DispatchedProduct> dispatchedProductsByType = dispatchedProducts.Where(x => x.Type == type).ToList();
                    List<ReturnedProduct> returnedProductsByType = returnedProducts.Where(x => x.Type == type).ToList();
                    List<ClientProduct> clientStockByType = clientStock.Where(x => x.Product.Type == type).ToList();
                    soldProducts.Add(new SoldProductsTable
                    {
                        Name = type.GetDisplayName(),
                        Sold = cartProductsByType != null ? cartProductsByType.Sum(x => x.Quantity) : 0,
                        Dispatched = dispatchedProductsByType != null ? dispatchedProductsByType.Sum(x => x.Quantity) : 0,
                        Returned = returnedProductsByType != null ? returnedProductsByType.Sum(x => x.Quantity) : 0,
                        ClientStock = clientStockByType != null ? clientStockByType.Sum(x => x.Stock) : 0,
                    });
                }
                return soldProducts;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}