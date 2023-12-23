using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;

namespace AguasNico.Data.Repository
{
    public class ApplicationUserRepository(ApplicationDbContext db) : Repository<ApplicationUser>(db), IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db = db;

        public IEnumerable<ApplicationUser> GetDealers()
        {
            return _db.UserRoles.Where(x => x.RoleId.Equals(_db.Roles.First(x => x.Name.Equals(Constants.Dealer)).Id)).Select(x => x.UserId).Select(x => _db.User.First(y => y.Id.Equals(x)));
        }

        public IdentityRole GetRole(string userID)
        {
            return _db.Roles.First(x => x.Id.Equals(_db.UserRoles.First(x => x.UserId.Equals(userID)).RoleId));
        }

        public IEnumerable<SelectListItem> GetDropDownList()
        {
            IEnumerable<SelectListItem> users = new List<SelectListItem>
            {
                new() { Value = "", Text = "Seleccione un usuario", Disabled = true, Selected = true }
            };
            return users.Concat(_db.User.OrderBy(x => x.UserName).Select(i => new SelectListItem()
            {
                Text = i.UserName,
                Value = i.Id,
            }));
        }

        public IEnumerable<SelectListItem> GetDealersDropDownList()
        {
            IEnumerable<SelectListItem> users = new List<SelectListItem>
            {
                new() { Value = "", Text = "Seleccione un repartidor", Disabled = true, Selected = true }
            };
            return users.Concat(_db.UserRoles.Where(x => x.RoleId.Equals(_db.Roles.First(x => x.Name.Equals(Constants.Dealer)).Id)).Select(x => x.UserId).Select(x => _db.User.First(y => y.Id.Equals(x))).OrderBy(x => x.UserName).Select(i => new SelectListItem()
            {
                Text = i.UserName,
                Value = i.Id,
            }));
        }
    }
}