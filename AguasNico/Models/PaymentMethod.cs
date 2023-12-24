using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AguasNico.Models
{
    public class PaymentMethod
    {
        [Key]
        public short ID { get; set; }

        [Display(Name = "Método de pago")]
        public string Name { get; set; } = null!;

        [Display(Name = "Habilitado")]
        public bool IsActive { get; set; } = true;

        public virtual IEnumerable<CartPaymentMethod> Carts { get; set; } = null!;
    }
}
