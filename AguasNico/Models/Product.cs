using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AguasNico.Models
{
    public enum ProductType
    {
        [Display(Name = "Bidón 20L")] B20L = 1,
        [Display(Name = "Bidón 12")] B12L = 2,
        [Display(Name = "Soda")] Soda = 3,
        [Display(Name = "Máquina frío calor")] Máquina = 4,
    }
    public class Product
    {
        [Key]
        public long ID { get; set; }

        [Required(ErrorMessage = "Debes ingresar un nombre")]
        [Display(Name = "Nombre")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Debes ingresar un nombre de menos de 200 caracteres")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Debes ingresar un precio")]
        [Display(Name = "Precio")]
        [Column(TypeName = "money")]
        [Range(0, 1000000, ErrorMessage = "Debes ingresar un precio válido")]
        [DisplayFormat(DataFormatString = "{0:F0}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Debes ingresar un tipo de producto")]
        [Display(Name = "Tipo de producto")]
        public ProductType Type { get; set; }
        
        [Display(Name = "¿Está activo?")]
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }
    }
}
