
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

        [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal TransferAmount { get; set; } = 0;
        
        [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal AbonoPrice { get; set; } = 0;
    }
}
