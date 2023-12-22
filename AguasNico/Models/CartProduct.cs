using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AguasNico.Models
{
    public class CartProduct
    {
        [Required]
        public long CartID { get; set; }

        [Required]
        public long ProductID { get; set; }

        [Required(ErrorMessage = "Debes ingresdar una cantidad")]
        [Display(Name = "Cantidad")]
        [Range(1, 100, ErrorMessage = "La cantidad debe estar entre 1 y 100")]
        public int Quantity { get; set; }

        [Display(Name = "Precio")]
        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0:F0}", ApplyFormatInEditMode = true)]
        public decimal SettedPrice { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        public virtual Cart Cart { get; set; } = null!;
        
        public virtual Product Product { get; set; } = null!;
    }
}
