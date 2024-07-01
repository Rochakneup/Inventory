using Inventory.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Inventory.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace Inventory.Areas.Identity.Pages
{
    [Authorize(Roles = "Admin")]
    public class ResetPasswordAdminModel : PageModel
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly ILogger<ResetPasswordAdminModel> _logger;

        public ResetPasswordAdminModel(UserManager<AuthUser> userManager, ILogger<ResetPasswordAdminModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Temporary Password")]
            public string TemporaryPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm New Password")]
            [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; } // This will hold the token generated for password reset

            public string Email { get; set; }
        }

        public IActionResult OnGet(string code = null, string email = null)
        {
            if (code == null || email == null)
            {
                return BadRequest("A code and email must be supplied for password reset.");
            }
            else
            {
                Input = new InputModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)),
                    Email = email
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Find user by email
            var user = await _userManager.FindByEmailAsync(Input.Email);

            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("/Index");
            }

            // Verify the temporary password first
            var isTempPasswordValid = await _userManager.CheckPasswordAsync(user, Input.TemporaryPassword);
            if (!isTempPasswordValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid temporary password.");
                return Page();
            }

            // Reset user's password to the new password
            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);

            if (resetPasswordResult.Succeeded)
            {
                
                _logger.LogInformation("User password reset successfully.");
                user.EmailConfirmed = true;

                // Optionally update the user entity to reflect password change
                await _userManager.UpdateAsync(user);

                return RedirectToPage("/Index");
            }

            foreach (var error in resetPasswordResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}