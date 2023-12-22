using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AguasNico.Models
{
    public class Expense
    {
        [Key]
        public long ID { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required(ErrorMessage = "Debes seleccionar un repartidor")]
        [Display(Name = "Monto")]
        [Column(TypeName = "money")]
        [Range(0, 1000000, ErrorMessage = "El monto debe ser mayor a 0")]
        [DisplayFormat(DataFormatString = "{0:F0}", ApplyFormatInEditMode = true)]
        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;
    }
}
