using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetCare.Core.Models;

namespace PetCare.WebApp.Pages.Admin.Users
{
    [Authorize(Roles = "Admin")]
    public class ManageRolesModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageRolesModel(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public User TargetUser { get; set; }

        [BindProperty]
        public List<RoleSelection> RolesList { get; set; } = new();

        public class RoleSelection
        {
            public string RoleName { get; set; }
            public bool IsSelected { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            TargetUser = await _userManager.FindByIdAsync(userId);
            if (TargetUser == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(TargetUser);
            var allRoles = _roleManager.Roles.ToList();

            foreach (var role in allRoles)
            {
                RolesList.Add(new RoleSelection
                {
                    RoleName = role.Name,
                    IsSelected = userRoles.Contains(role.Name)
                });
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            // 1. Dodaj nowo zaznaczone
            var rolesToAdd = RolesList.Where(x => x.IsSelected && !userRoles.Contains(x.RoleName))
                                      .Select(x => x.RoleName);
            if (rolesToAdd.Any()) await _userManager.AddToRolesAsync(user, rolesToAdd);

            // 2. Usuñ odznaczone
            var rolesToRemove = RolesList.Where(x => !x.IsSelected && userRoles.Contains(x.RoleName))
                                         .Select(x => x.RoleName);
            if (rolesToRemove.Any()) await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            return RedirectToPage("./Index");
        }
    }
}