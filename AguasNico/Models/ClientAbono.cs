using System.ComponentModel.DataAnnotations;

namespace AguasNico.Models
{
    public class ClientAbono
    {
        [Required]
        public long ClientID { get; set; }

        [Required]
        public long AbonoID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        public virtual Product Product { get; set; } = null!;

        public virtual Abono Abono { get; set; } = null!;
    }
}
