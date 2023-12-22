using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AguasNico.Models
{
    public class CartPaymentMethod
    {
        [Required]
        public long CartID { get; set; }

        [Required]
        public short PaymentMethodID { get; set; }

        [Required(ErrorMessage = "Debes ingresar un monto")]
        [Display(Name = "Monto")]
        [Column(TypeName = "money")]
        [Range(0, 1000000, ErrorMessage = "Debes ingresar un monto entre $0 y $1.000.000")]
        [DisplayFormat(DataFormatString = "{0:F0}", ApplyFormatInEditMode = true)]
        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        public virtual Cart Cart { get; set; } = null!;
        public virtual PaymentMethod PaymentMethod { get; set; } = null!;
    }
}
