using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetCare.Core.Models;

namespace PetCare.WebApp.Pages.Admin.Users
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IndexModel(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public class UserViewModel
        {
            public User User { get; set; }
            public IList<string> Roles { get; set; }
        }

        public List<UserViewModel> UsersWithRoles { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? FilterRole { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? FilterStatus { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SortColumn { get; set; } = "Email";

        [BindProperty(SupportsGet = true)]
        public string? SortDirection { get; set; } = "asc";

        public SelectList RoleFilterOptions { get; set; }
        public async Task OnGetAsync()
        {
            var roles = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
            RoleFilterOptions = new SelectList(roles, "Name", "Name");

            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                var term = SearchTerm.ToLower();
                query = query.Where(u => u.Email.ToLower().Contains(term) || u.UserName.ToLower().Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(FilterStatus) && FilterStatus != "all")
            {
                bool isActive = FilterStatus == "active";
                query = query.Where(u => u.IsActive == isActive);
            }

            bool isDesc = SortDirection?.ToLower() == "desc";

            query = SortColumn switch
            {
                "Active" => isDesc ? query.OrderByDescending(u => u.IsActive) : query.OrderBy(u => u.IsActive),
                _ => isDesc ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email)
            };

            var usersList = await query.ToListAsync();

            foreach (var user in usersList)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                if (!string.IsNullOrWhiteSpace(FilterRole) && !userRoles.Contains(FilterRole))
                {
                    continue;
                }

                UsersWithRoles.Add(new UserViewModel { User = user, Roles = userRoles });
            }
        }

        public string GetNextSort(string column)
        {
            if (SortColumn == column)
                return SortDirection == "asc" ? "desc" : "asc";
            return "asc";
        }

        public string GetSortIcon(string column)
        {
            if (SortColumn != column) return "bi-arrow-down-up text-muted opacity-25";
            return SortDirection == "asc" ? "bi-arrow-up-short" : "bi-arrow-down-short";
        }
    }
}