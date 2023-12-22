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
        public IdentityRole GetRole(string userID)
        {
            return _db.Roles.First(x => x.Id.Equals(_db.UserRoles.First(x => x.UserId.Equals(userID)).RoleId));
        }
    }
}