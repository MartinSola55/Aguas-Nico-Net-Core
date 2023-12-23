using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Models;

namespace AguasNico.Data.Repository.IRepository
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        IEnumerable<ApplicationUser> GetDealers();
        IdentityRole GetRole(string userID);
        IEnumerable<SelectListItem> GetDropDownList();
        IEnumerable<SelectListItem> GetDealersDropDownList();
    }
}