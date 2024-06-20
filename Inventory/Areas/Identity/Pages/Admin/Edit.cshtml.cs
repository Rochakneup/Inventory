using Inventory.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Inventory.Areas.Identity.Pages.Admin
{
    public class EditModel : PageModel
    {
        private readonly UserManager<AuthUser> _userManager;

        public EditModel(UserManager<AuthUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public EditUserInputModel Input { get; set; }

        public class EditUserInputModel
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Status { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            Input = new EditUserInputModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Status = user.Status // Set the status property
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(Input.Id);

                if (user == null)
                {
                    return NotFound();
                }

                user.UserName = Input.UserName;
                user.Email = Input.Email;
                user.Status = Input.Status; // Update the status property

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToPage("/Admin/Dashboard", new { area = "Identity" });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}
