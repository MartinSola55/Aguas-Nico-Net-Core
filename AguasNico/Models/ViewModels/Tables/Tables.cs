﻿
using System.ComponentModel.DataAnnotations;

namespace AguasNico.Models.ViewModels.Tables
{
    public class InvoiceProduct
    {
        public string Type { get; set; } = "";
        public int Quantity { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,##0}")]
        public decimal Total { get; set; }
    }
    public class InvoiceTable
    {
        public Client Client { get; set; } = null!;
        public List<InvoiceProduct> Products { get; set; } = [];
    }
    public class SoldProductsTable
    {
        public string Name { get; set; } = null!;
        public int Dispatched { get; set; } = 0;
        public int Sold { get; set; } = 0;
        public int Returned { get; set; } = 0;
        public int ClientStock { get; set; } = 0;
    }

    public enum CartsTransfersType
    {
        [Display(Name = "Transferencia")] Transfer,
        [Display(Name = "Carrito")] Cart,
        [Display(Name = "Abono")] Abono
    }
    public class CartsTransfersHistoryTable
    {
        public DateTime Date { get; set; }
        public CartsTransfersType Type { get; set; }
        public State CartState { get; set; }
        public string AbonoName { get; set; } = null!;
        public List<CartPaymentMethod> PaymentMethods { get; set; } = [];
        public List<CartProduct> Products { get; set; } = [];
        public List<CartAbonoProduct> AbonoProducts { get; set; } = [];

        [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal TransferAmount { get; set; } = 0;

        [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal AbonoPrice { get; set; } = 0;
    }

    public class DealerSheet
    {
        public Day Day { get; set; }
        public long ClientID { get; set; }
        public string ClientName { get; set; } = "";
        public string ClientPhone { get; set; } = "";
        public string ClientAddress { get; set; } = "";
        public string ClientObservations { get; set; } = "";

        [DisplayFormat(DataFormatString = "${0:#,##0}")]
        public decimal ClientDebt { get; set; }
        public List<Product> Products { get; set; } = [];
        public List<AbonoProduct> AbonoProducts { get; set; } = [];
        public List<AbonoRenewal> Abonos { get; set; } = [];

        public class Product
        {
            public ProductType Type { get; set; }

            [DisplayFormat(DataFormatString = "${0:#,##0}")]
            public decimal Price { get; set; }
            public int Stock { get; set; }
        }
        public class AbonoProduct
        {
            public long AbonoID { get; set; }
            public ProductType Type { get; set; }
            public int Available { get; set; }
            public int Stock { get; set; }
        }
    }
}
