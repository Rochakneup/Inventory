using Inventory.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Areas.Identity.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly UserManager<AuthUser> _userManager;

        public DashboardModel()
        {

        }

        public async Task OnGet()
        {
          
        }
    }
}
