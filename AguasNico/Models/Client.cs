using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AguasNico.Models
{
    public class Client
    {
        [Key]
        public long ID { get; set; }

        [Required(ErrorMessage = "Debes ingresar un nombre")]
        [Display(Name = "Nombre")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Debes ingresar un nombre de menos de 200 caracteres")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Debes ingresar una dirección")]
        [Display(Name = "Dirección")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Debes ingresar una dirección de menos de 200 caracteres")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Debes ingresar un teléfono")]
        [Display(Name = "Teléfono")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Debes ingresar un teléfono de menos de 200 caracteres")]
        [Phone(ErrorMessage = "Debes ingresar un teléfono válido")]
        public string Phone { get; set; } = null!;

        [Display(Name = "Observaciones")]
        [StringLength(300, MinimumLength = 1, ErrorMessage = "Debes ingresar observaciones de menos de 300 caracteres")]
        public string? Observations { get; set; } = null!;

        [Display(Name = "Deuda")]
        [Column(TypeName = "money")]
        [DefaultValue(0)]
        [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal Debt { get; set; } = 0;

        [Display(Name = "Repartidor")]
        public string? DealerID { get; set; } = null!;

        [Display(Name = "¿Quiere factura?")]
        [DefaultValue(false)]
        public bool HasInvoice { get; set; } = false;

        [Display(Name = "Tipo de factura")]
        public InvoiceType? InvoiceType { get; set; } = null!;

        [Display(Name = "Condición frente al IVA")]
        public TaxCondition? TaxCondition { get; set; } = null!;

        [Display(Name = "CUIT")]
        [StringLength(11, MinimumLength = 1, ErrorMessage = "Debes ingresar un CUIT de menos de 12 caracteres")]
        public string? CUIT { get; set; } = null!;

        [Display(Name = "Día del reparto")]
        public Day? DeliveryDay { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;

        public virtual ApplicationUser Dealer { get; set; } = null!;

        public virtual ICollection<ClientProduct> Products { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Cart> Carts { get; set; } = null!;
        
        [JsonIgnore]
        public virtual ICollection<ClientAbono> Abonos { get; set; } = null!;

        public virtual ICollection<Transfer> Transfers { get; set; } = null!;


        // Not mapped properties
        // [NotMapped]
        // public DateTime? LastCart 
        // {
        //     get
        //     {
        //         return GetLastCart();
        //     }
        // }
        // internal DateTime? GetLastCart()
        // {
        //     Cart? lastCart = Carts.Where(x => x.State == State.Confirmed && x.ClientID == this.ID).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
        //     if (lastCart is null)
        //     {
        //         return null;
        //     }
        //     return lastCart.CreatedAt;
        // }

        // [NotMapped]
        // public decimal DebtOfTheMonth
        // {
        //     get
        //     {
        //         return GetDebtOfTheMonth();
        //     }
        // }
        // internal decimal GetDebtOfTheMonth() // CONSUMO DEL MES
        // {
        //     decimal total = 0;
        //     DateTime today = DateTime.UtcNow.AddHours(-3);
        //     foreach (Cart cart in Carts.Where(x => x.State == State.Confirmed && x.ClientID == this.ID && x.CreatedAt.Month == today.Month && x.CreatedAt.Year == today.Year))
        //     {
        //         foreach (CartProduct product in cart.Products)
        //         {
        //             total += product.Quantity * product.SettedPrice;
        //         }
        //     }
        //     // TODO: Sumar abono
        //     return total;
        // }

        // [NotMapped]
        // public decimal DebtOfPreviousMonth // CONSUMO DEL MES ANTERIOR
        // {
        //     get
        //     {
        //         return GetDebtOfPreviousMonth();
        //     }
        // }
        // internal decimal GetDebtOfPreviousMonth()
        // {
        //     decimal total = 0;
        //     DateTime today = DateTime.UtcNow.AddHours(-3).AddMonths(-1);
        //     foreach (Cart cart in Carts.Where(x => x.State == State.Confirmed && x.ClientID == this.ID && x.CreatedAt.Month == today.Month && x.CreatedAt.Year == today.Year))
        //     {
        //         foreach (CartProduct product in cart.Products)
        //         {
        //             total += product.Quantity * product.SettedPrice;
        //         }
        //     }
        //     // TODO: Sumar abono
        //     return total;
        // }
    }
}
