using Microsoft.AspNetCore.Identity;

namespace PetCare.Infrastructure.Data
{
    public static class IdentitySeed
    {
        public const string RoleAdmin = "Admin";
        public const string RoleEmployee = "Employee";
        public const string RoleClient = "Client";

        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(RoleAdmin))
            {
                await roleManager.CreateAsync(new IdentityRole(RoleAdmin));
            }

            if (!await roleManager.RoleExistsAsync(RoleEmployee))
            {
                await roleManager.CreateAsync(new IdentityRole(RoleEmployee));
            }

            if (!await roleManager.RoleExistsAsync(RoleClient))
            {
                await roleManager.CreateAsync(new IdentityRole(RoleClient));
            }
        }
    }
}
