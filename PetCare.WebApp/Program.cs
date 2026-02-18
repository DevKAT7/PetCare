using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Extensions;
using PetCare.Application.Interfaces;
using PetCare.Infrastructure.Data;
using PetCare.Infrastructure.Extensions;
using PetCare.Infrastructure.Services;
using Serilog;
using System.Globalization;

namespace PetCare.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/log-webapp-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Log.Information("Starting PetCare WebApp...");

                var builder = WebApplication.CreateBuilder(args);

                builder.Host.UseSerilog();

                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

                builder.Services.AddHttpContextAccessor();

                builder.Services.AddApplicationLayer();
                builder.Services.AddInfrastructureLayer(builder.Configuration);

                builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

                builder.Services.AddScoped<IDocumentGenerator, DocumentGenerator>();

                builder.Services.AddDatabaseDeveloperPageExceptionFilter();
                builder.Services.AddRazorPages();

                builder.Services.AddFluentValidationAutoValidation();
                builder.Services.AddFluentValidationClientsideAdapters();

                builder.Services.ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = "/Identity/Account/Login";
                    options.LogoutPath = "/Identity/Account/Logout";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                });

                var cultureInfo = new CultureInfo("en-US");
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseMigrationsEndPoint();
                }
                else
                {
                    app.UseExceptionHandler("/Error");
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseRouting();

                app.UseAuthorization();

                app.MapRazorPages();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "WebApp terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}