using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AguasNico.Models
{
    public class Abono
    {
        [Key]
        public long ID { get; set; }

        [Required(ErrorMessage = "Debes ingresar un nombre")]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Debes ingresar un precio")]
        [Display(Name = "Precio")]
        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        public virtual IEnumerable<AbonoProduct> Products { get; set; } = null!;
        public virtual IEnumerable<AbonoRenewal> Renewals { get; set; } = null!;
    }
}
