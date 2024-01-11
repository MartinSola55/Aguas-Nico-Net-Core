using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AguasNico.Models
{
    public class AbonoProduct
    {
        [Required]
        public long AbonoID { get; set; }

        [Required]
        public ProductType Type { get; set; }

        [Required(ErrorMessage = "Debes ingresdar una cantidad")]
        [Display(Name = "Cantidad")]
        [Range(1, 100, ErrorMessage = "La cantidad debe estar entre 1 y 100")]
        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        [JsonIgnore]
        public virtual Abono Abono { get; set; } = null!;
    }
}
