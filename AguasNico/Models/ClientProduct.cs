using System.ComponentModel.DataAnnotations;

namespace AguasNico.Models
{
    public class ClientProduct
    {
        [Required]
        public long ClientID { get; set; }

        [Required]
        public long ProductID { get; set; }

        [Required(ErrorMessage = "Debes ingresar un stock")]
        [Range(0, 200, ErrorMessage = "El stock debe estar entre 0 y 200")]
        public int Stock { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        public virtual Product Product { get; set; } = null!;

        public virtual Client Client { get; set; } = null!;
    }
}
