using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetCare.Core.Models;
using PetCare.Infrastructure.Data;
using System.ComponentModel.DataAnnotations;

namespace PetCare.WebApp.Pages.Admin.Vets
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public ResetPasswordModel(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public string VetName { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var vet = await _context.Vets
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.VetId == id);

            if (vet == null || vet.User == null)
            {
                return NotFound();
            }

            VetName = $"{vet.FirstName} {vet.LastName}";
            Input = new InputModel { UserId = vet.UserId, VetId = id };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByIdAsync(Input.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, Input.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            user.RequirePasswordChange = true;
            await _userManager.UpdateAsync(user);

            TempData["SuccessMessage"] = "Password has been reset successfully. The user must change it upon next login.";

            return RedirectToPage("./EditVet", new { id = Input.VetId });
        }

        public class InputModel
        {
            [Required]
            public string UserId { get; set; }

            public int VetId { get; set; }

            [Required]
            [StringLength(100, MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "New Temporary Password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare("NewPassword", ErrorMessage = "The passwords do not match.")]
            public string ConfirmPassword { get; set; }
        }
    }
}
