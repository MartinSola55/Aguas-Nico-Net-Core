using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using Microsoft.EntityFrameworkCore;

namespace AguasNico.Data.Repository
{
    public class ApplicationUserRepository(ApplicationDbContext db) : Repository<ApplicationUser>(db), IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db = db;

        public async Task<List<ApplicationUser>> GetDealers()
        {
            var dealer = await _db.Roles.FirstAsync(x => x.Name != null && x.Name.Equals(Constants.Dealer));
            return await _db
                .UserRoles
                .Where(x => x.RoleId.Equals(dealer.Id))
                .Select(x => x.UserId)
                .Select(x => _db.User.First(y => y.Id.Equals(x)))
                .ToListAsync();
        }

        public async Task<IdentityRole> GetRole(string userID)
        {
            var role = await _db.UserRoles.FirstAsync(x => x.UserId.Equals(userID));
            return await _db
                .Roles
                .FirstAsync(x => x.Id.Equals(role.RoleId));
        }

        public async Task<List<SelectListItem>> GetDropDownList()
        {
            var dropDown = new List<SelectListItem>
            {
                new() { Value = "", Text = "Seleccione un usuario", Disabled = true, Selected = true }
            };
            var users = await _db
                .User
                .OrderBy(x => x.UserName)
                .Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id,
                })
                .ToListAsync();

            return [.. dropDown, .. users];
        }

        public async Task<List<SelectListItem>> GetDealersDropDownList()
        {
            var dropDown = new List<SelectListItem>
            {
                new() { Value = "", Text = "Seleccione un repartidor", Disabled = true, Selected = true }
            };

            var role = await _db.Roles.FirstAsync(x => x.Name != null && x.Name.Equals(Constants.Dealer));

            var usersIds = await _db
                .UserRoles
                .Where(x => x.RoleId.Equals(role.Id))
                .Select(x => x.UserId)
                .ToListAsync();
            
            var users = await _db
                .User
                .Where(x => usersIds.Contains(x.Id))
                .OrderBy(x => x.TruckNumber)
                .Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id,
                })
                .ToListAsync();

            return [.. dropDown, .. users];
        }
    }
}