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
        Task<List<ApplicationUser>> GetDealers();
        Task<IdentityRole> GetRole(string userID);
        Task<List<SelectListItem>> GetDropDownList();
        Task<List<SelectListItem>> GetDealersDropDownList();
    }
}