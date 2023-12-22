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
                ApplicationUser user = new()
                {
                    UserName = _config["UserEmail"],
                    Email = _config["UserEmail"],
                    EmailConfirmed = true,
                };
                _userManager.CreateAsync(user, _config["UserPassword"]).GetAwaiter().GetResult();

                ApplicationUser newUser = _db.User.Where(u => u.UserName.Equals(user.UserName)).First();

                _userManager.AddToRoleAsync(newUser, Constants.Admin).GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
