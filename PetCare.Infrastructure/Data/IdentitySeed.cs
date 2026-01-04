using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PetCare.Core.Models;

namespace PetCare.Infrastructure.Data
{
    public static class IdentitySeed
    {
        public const string RoleAdmin = "Admin";
        public const string RoleEmployee = "Employee";
        public const string RoleClient = "Client";

        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            await SeedRolesAsync(roleManager);

            await SeedAdminAsync(userManager);
        }

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

        private static async Task SeedAdminAsync(UserManager<User> userManager)
        {
            var adminEmail = "admin@petcare.pl"; // Można pobrać z konfiguracji

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    IsActive = true,
                };

                // UWAGA: W pracy inżynierskiej zostawiłam to hasło w kodzie
                // ale wiem, że na produkcji robi się to inaczej i dałabym je do sekretów
                string adminPassword = "StrongPassword123!";

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, RoleAdmin);
                }
            }
        }
    }
}
