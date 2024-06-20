using Inventory.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Inventory.Areas.Identity.Pages.Admin
{
    public class DeleteModel : PageModel
    {
        private readonly UserManager<AuthUser> _userManager;

        public DeleteModel(UserManager<AuthUser> userManager)
        {
            _userManager = userManager;
        }

        public string UserId { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            UserId = user.Id;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToPage("/Admin/Dashboard", new { area = "Identity" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
