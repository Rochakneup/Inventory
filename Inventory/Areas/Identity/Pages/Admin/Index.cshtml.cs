using Inventory.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Areas.Identity.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AuthUser> _userManager;

        public IndexModel(UserManager<AuthUser> userManager)
        {
            _userManager = userManager;
        }

        public List<AuthUser> Users { get; set; }

        public async Task OnGetAsync()
        {
            Users = await _userManager.Users.ToListAsync();
        }
    }
}
