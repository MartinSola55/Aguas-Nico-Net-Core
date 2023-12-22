using Microsoft.AspNetCore.Identity;

namespace AguasNico.Models
{
    public partial class ApplicationUser : IdentityUser
    {
        public int? TruckNumber { get; set; }
    }
}
