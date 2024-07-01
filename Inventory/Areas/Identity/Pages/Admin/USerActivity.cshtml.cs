using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Areas.Identity.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class UserActivityModel : PageModel
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserActivityModel(UserManager<AuthUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public IList<AuthUser> Users { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<AuthUser> usersQuery = _userManager.Users;

            if (!string.IsNullOrEmpty(SearchString))
            {
                usersQuery = usersQuery.Where(u => u.UserName.Contains(SearchString)
                                                || u.Email.Contains(SearchString)
                                                || (u.Firstname + " " + u.Lastname).Contains(SearchString));
            }

            Users = await usersQuery.ToListAsync();
        }

        public async Task<IList<string>> GetUserRolesAsync(AuthUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }
    }
}
