using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AguasNico.Models
{
    public class ReturnedProduct
    {
        [Required]
        public ProductType Type { get; set; }

        [Required]
        public long CartID { get; set; }

        [Required(ErrorMessage = "Debes ingresar una cantidad")]
        [Display(Name = "Cantidad")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        [JsonIgnore]
        public virtual Cart Cart { get; set; } = null!;
    }
}
