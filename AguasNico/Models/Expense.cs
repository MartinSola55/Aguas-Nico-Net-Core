﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AguasNico.Models
{
    public class Expense
    {
        [Key]
        public long ID { get; set; }

        [Required]
        [Display(Name = "Repartidor")]
        public string UserID { get; set; } = null!;

        [Required(ErrorMessage = "Debes seleccionar un repartidor")]
        [Display(Name = "Monto")]
        [Column(TypeName = "money")]
        [Range(0, 10000000, ErrorMessage = "El monto debe ser mayor a 0")]
        [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Debes ingresar una descripción")]
        [Display(Name = "Descripción")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Debes ingresar una descripción de menos de 200 caracteres")]
        public string Description { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;
    }
}
