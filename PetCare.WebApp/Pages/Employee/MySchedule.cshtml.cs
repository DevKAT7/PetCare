using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetCare.Core.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.WebApp.Pages.Employee
{
    [Authorize(Roles = "Employee")]
    public class MyScheduleModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public MyScheduleModel(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var vet = await _context.Vets
                .FirstOrDefaultAsync(v => v.UserId == user.Id);

            if (vet == null)
            {
                return NotFound("No vet profile found for this user.");
            }

            return RedirectToPage("/ManageSchedule", new { vetId = vet.VetId });
        }
    }
}
