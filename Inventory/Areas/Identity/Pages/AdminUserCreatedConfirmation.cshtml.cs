using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Inventory.Areas.Identity.Pages
{
    [Authorize(Roles = "Admin")]
    public class AdminUserCreatedConfirmationModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
