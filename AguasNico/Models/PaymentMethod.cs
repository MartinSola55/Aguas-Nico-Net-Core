using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AguasNico.Models
{
    public class PaymentMethod
    {
        [Key]
        public short ID { get; set; }

        [Display(Name = "Método de pago")]
        public string Name { get; set; } = null!;

        [JsonIgnore]
        public virtual IEnumerable<CartPaymentMethod> Carts { get; set; } = null!;
    }
}
