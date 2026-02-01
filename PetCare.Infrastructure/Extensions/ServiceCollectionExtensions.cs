using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetCare.Application.Interfaces;
using PetCare.Core.Models;
using PetCare.Infrastructure.Data;
using PetCare.Infrastructure.Services;

namespace PetCare.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            // Rejestracja Identity (Wspólna baza)
            // WAŻNE: Tutaj konfigurujemy tylko "wnętrze" Identity (Stores, Managers).
            // Sposób uwierzytelniania (Cookies vs Tokeny) konfigurujemy osobno w każdym Program.cs!
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddHostedService<NotificationBackgroundService>();
            services.AddScoped<IFileStorageService, FileStorageService>();

            return services;
        }
    }
}
