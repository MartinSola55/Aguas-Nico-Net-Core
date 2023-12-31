
using System.ComponentModel.DataAnnotations;

namespace AguasNico.Models.ViewModels.Tables
{
    public class SoldProductsTable
    {
        public string Name { get; set; } = null!;
        public int Dispatched { get; set; } = 0;
        public int Sold { get; set; } = 0;
        public int Returned { get; set; } = 0;
    }

    public enum CartsTransfersType
    {
        [Display(Name = "Transferencia")] Transfer,
        [Display(Name = "Carrito")] Cart
    }
    public class CartsTransfersHistoryTable
    {
        public DateTime Date { get; set; }
        public CartsTransfersType Type { get; set; }
        public List<CartPaymentMethod> PaymentMethods { get; set; } = [];
        public List<CartProduct> Products { get; set; } = [];

        [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal TransferAmount { get; set; } = 0;
    }
}
