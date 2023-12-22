using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AguasNico.Models
{
    public enum InvoiceType
    {
        A = 1,
        B = 2
    }
    public enum TaxCondition
    {
        RI = 1,
        MO = 2,
        EX = 3,
        CF = 4,
    }
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
        [DisplayFormat(DataFormatString = "{0:F0}", ApplyFormatInEditMode = true)]
        public decimal Debt { get; set; } = 0;

        public string? DealerID { get; set; } = null!;

        [Display(Name = "¿Quiere factura?")]
        [DefaultValue(false)]
        public bool HasInvoice { get; set; } = false;

        [Display(Name = "¿Está activo?")]
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Tipo de factura")]
        public InvoiceType? InvoiceType { get; set; } = null!;

        [Display(Name = "Condición frente al IVA")]
        public TaxCondition? TaxCondition { get; set; } = null!;

        [Display(Name = "CUIT")]
        [StringLength(11, MinimumLength = 1, ErrorMessage = "Debes ingresar un CUIT de menos de 12 caracteres")]
        public string? CUIT { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        public virtual ApplicationUser Dealer { get; set; } = null!;

        public virtual ICollection<ClientProduct> ClientProducts { get; set; } = null!;

        public virtual ICollection<Cart> Carts { get; set; } = null!;

        public virtual ICollection<Transfer> Transfers { get; set; } = null!;
    }
}
