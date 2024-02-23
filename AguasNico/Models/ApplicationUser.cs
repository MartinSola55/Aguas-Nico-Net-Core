using Microsoft.AspNetCore.Identity;

namespace AguasNico.Models
{
    public partial class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = "";
        public int? TruckNumber { get; set; }
    }
}
