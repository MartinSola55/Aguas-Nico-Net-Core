using System.ComponentModel.DataAnnotations;

namespace AguasNico.Models
{
    public class DispatchedProduct
    {
        [Required]
        public long RouteID { get; set; }
        
        [Required]
        public ProductType Type { get; set; }

        [Required(ErrorMessage = "Debes ingresar una cantidad")]
        [Display(Name = "Cantidad")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 0")]
        public int Quantity { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        public virtual Route Route { get; set; } = null!;
    }
}
