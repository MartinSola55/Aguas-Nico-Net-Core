using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AguasNico.Models;
using System.Data.Common;
using System.Reflection.Emit;

namespace AguasNico.Data.Seeding
{
    public class Seeder(ApplicationDbContext db, IConfiguration config, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : ISeeder
    {
        private readonly ApplicationDbContext _db = db;
        private readonly IConfiguration _config = config;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public void Seed()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }

                if (_db.Roles.Any(x => x.Name == Constants.Admin)) return;

                // Crear roles
                _roleManager.CreateAsync(new IdentityRole(Constants.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Constants.Dealer)).GetAwaiter().GetResult();

                // Crear usuarios
                ApplicationUser admin = new()
                {
                    UserName = _config["AdminEmail"],
                    Email = _config["AdminEmail"],
                    EmailConfirmed = true,
                };
                ApplicationUser dealer = new()
                {
                    UserName = _config["DealerEmail"],
                    Email = _config["DealerEmail"],
                    EmailConfirmed = true,
                };
                _userManager.CreateAsync(admin, _config["AdminPassword"]).GetAwaiter().GetResult();
                _userManager.CreateAsync(dealer, _config["DealerPassword"]).GetAwaiter().GetResult();

                ApplicationUser newAdmin = _db.User.Where(u => u.UserName.Equals(admin.UserName)).First();
                ApplicationUser newDealer = _db.User.Where(u => u.UserName.Equals(dealer.UserName)).First();

                _userManager.AddToRoleAsync(newAdmin, Constants.Admin).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(newDealer, Constants.Dealer).GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
