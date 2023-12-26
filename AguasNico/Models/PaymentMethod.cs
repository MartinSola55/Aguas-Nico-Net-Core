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

        public virtual IEnumerable<CartPaymentMethod> Carts { get; set; } = null!;
    }
}
