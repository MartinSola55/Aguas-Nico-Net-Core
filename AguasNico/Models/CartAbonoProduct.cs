using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AguasNico.Models
{
    public class CartAbonoProduct
    {
        [Required]
        public long CartID { get; set; }

        [Required]
        public AbonoRenewalProduct Product { get; set; } = new();

        [Required(ErrorMessage = "Debes ingresdar una cantidad")]
        [Display(Name = "Cantidad")]
        [Range(1, 100, ErrorMessage = "La cantidad debe estar entre 1 y 100")]
        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        [JsonIgnore]
        public virtual Cart Cart { get; set; } = null!;

        public virtual AbonoRenewalProduct AbonoRenewalProduct { get; set; } = null!;
    }
}
