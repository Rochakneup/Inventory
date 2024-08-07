using Inventory.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Inventory.Areas.Identity.Pages.Admin
{
    [Authorize(Roles = "Admin")]
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
                Status = user.Status
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

                // Validate if the username already exists
                var existingUserByUsername = await _userManager.FindByNameAsync(Input.UserName);
                if (existingUserByUsername != null && existingUserByUsername.Id != Input.Id)
                {
                    ModelState.AddModelError(string.Empty, "Username is already taken.");
                    return Page();
                }

                // Validate if the email already exists
                var existingUserByEmail = await _userManager.FindByEmailAsync(Input.Email);
                if (existingUserByEmail != null && existingUserByEmail.Id != Input.Id)
                {
                    ModelState.AddModelError(string.Empty, "Email is already in use.");
                    return Page();
                }

                // Prevent changing status if the user has never logged in
                if (user.LoginDate == null && user.Status == "Inactive" && Input.Status != "Inactive")
                {
                    ModelState.AddModelError(string.Empty, "Status cannot be changed because the user has never logged in.");
                    return Page();
                }

                user.UserName = Input.UserName;
                user.Email = Input.Email;
                user.Status = Input.Status;

                var result = await _userManager.UpdateAsync(user);

                TempData["message"] = "User edited successfully.";


                if (result.Succeeded)
                {
                    return RedirectToPage("/Admin/UserActivity", new { area = "Identity" });
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
