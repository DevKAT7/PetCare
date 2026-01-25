using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using PetCare.Api.Extensions;
using PetCare.Application.Extensions;
using PetCare.Infrastructure.Data;
using PetCare.Infrastructure.Extensions;
using Serilog;
using System.Text;

namespace PetCare.Api
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
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Log.Information("Running application PetCare API...");

                var builder = WebApplication.CreateBuilder(args);

                builder.Host.UseSerilog();

                // Add services to the container.
                builder.Services.AddApplicationLayer();
                builder.Services.AddInfrastructureLayer(builder.Configuration);
                builder.Services.AddControllers()
                    .AddJsonOptions(options => {
                        //konwersja Enumow na stringi w JSON
                        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                    });

                builder.Services.AddFluentValidationAutoValidation();

                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "PetCare API",
                        Version = "v1",
                        Description = "API for PetCare app"
                    });
                });

                var jwtSecret = builder.Configuration["JwtSettings:Secret"];

                if (string.IsNullOrEmpty(jwtSecret))
                {
                    throw new Exception("No JWT key found in appsettings.json!");
                }

                var key = Encoding.ASCII.GetBytes(jwtSecret);

                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

                var app = builder.Build();

                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    try
                    {
                        logger.LogInformation("Starting database initialization...");

                        var context = services.GetRequiredService<ApplicationDbContext>();

                        DomainSeed.SeedSpecializationsAsync(context).Wait();
                        logger.LogInformation("Specializations initialized.");

                        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                        IdentitySeed.SeedAsync(services).Wait();
                        logger.LogInformation("System roles initialized.");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "A critical error occurred during database initialization.");
                    }
                }

                app.UseGlobalExceptionHandler();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PetCare API");
                    });
                }

                app.UseHttpsRedirection();

                app.UseAuthorization();


                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
