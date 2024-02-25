using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AguasNico.Models
{
    public class AbonoRenewal
    {
        [Key]
        public long ID { get; set; }

        [Required]
        public long AbonoID { get; set; }

        [Required]
        public long ClientID { get; set; }

        [Display(Name = "Precio")]
        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "${0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal SettedPrice { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        [JsonIgnore]
        public virtual Abono Abono { get; set; } = null!;
        
        [JsonIgnore]
        public virtual Client Client { get; set; } = null!;

        public virtual List<AbonoRenewalProduct> ProductsAvailables { get; set; } = null!;
    }
}
