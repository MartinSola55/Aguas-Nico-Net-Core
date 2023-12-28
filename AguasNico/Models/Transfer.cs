using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AguasNico.Models
{
    public class Transfer
    {
        [Key]
        public long ID { get; set; }

        [Required]
        public long ClientID { get; set; }

        [Required]
        public string UserID { get; set; } = null!;

        [Required(ErrorMessage = "Debes ingresar un monto")]
        [Display(Name = "Monto")]
        [Range(1, 1000000, ErrorMessage = "El monto debe ser mayor a 0")]
        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Debes ingresar una fecha")]
        [Display(Name = "Fecha establecida")]
        public DateTime Date { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        [JsonIgnore]
        public virtual Client Client { get; set; } = null!;

        public virtual ApplicationUser User { get; set; } = null!;
    }
}
