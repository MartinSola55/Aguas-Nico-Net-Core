using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AguasNico.Models
{
    public class AbonoRenewalProduct
    {
        [Required]
        public long AbonoRenewalID { get; set; }

        [Required]
        public ProductType Type { get; set; }

        public int Available { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        public virtual AbonoRenewal AbonoRenewal { get; set; } = null!;
    }
}
