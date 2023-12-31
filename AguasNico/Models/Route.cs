using System.ComponentModel.DataAnnotations;

namespace AguasNico.Models
{
    public class Route
    {
        [Key]
        public long ID { get; set; }
        
        public string UserID { get; set; } = null!;

        [Required(ErrorMessage = "Debes ingresar un día de la semana")]
        [Display(Name = "Día")]
        public Day DayOfWeek { get; set; }

        public bool IsStatic { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;
        
        public virtual IEnumerable<Cart> Carts { get; set; } = null!;
        public virtual IEnumerable<DispatchedProduct> DispatchedProducts { get; set; } = null!;
    }
}
