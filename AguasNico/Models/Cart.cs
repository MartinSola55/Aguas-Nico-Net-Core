using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AguasNico.Models
{
    public enum State
    {
        Pending = 0,
        Confirmed = 1,
        Ausent = 2,
        NotNeeded = 3,
        Holidays = 4
    }
    public class Cart
    {
        [Key]
        public long ID { get; set; }

        [Required]
        public long ClientID { get; set; }
        
        [Required]
        public long RouteID { get; set; }

        public int Priority { get; set; }

        [Required]
        public State State { get; set; } = State.Pending;
        
        public bool IsStatic { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        public virtual Client Client { get; set; } = null!;
        
        public Route Route { get; set; } = null!;
    }
}
