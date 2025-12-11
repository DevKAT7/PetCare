using Microsoft.AspNetCore.Identity;
using PetCare.Application.Extensions;
using PetCare.Infrastructure.Data;
using PetCare.Infrastructure.Extensions;

namespace PetCare.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplicationLayer();
            builder.Services.AddInfrastructureLayer(builder.Configuration);

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddRazorPages();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    logger.LogInformation("Rozpoczynam inicjalizacjê bazy danych...");

                    var context = services.GetRequiredService<ApplicationDbContext>();

                    DomainSeed.SeedSpecializationsAsync(context).Wait();
                    logger.LogInformation("Specjalizacje zosta³y zainicjalizowane.");

                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    IdentitySeed.SeedRolesAsync(roleManager).Wait();
                    logger.LogInformation("Role systemowe zosta³y zainicjalizowane.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Wyst¹pi³ krytyczny b³¹d podczas inicjalizacji bazy danych.");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
