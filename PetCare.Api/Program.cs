using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi;
using PetCare.Api.Middleware;
using PetCare.Application.Extensions;
using PetCare.Infrastructure.Data;
using PetCare.Infrastructure.Extensions;

namespace PetCare.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddApplicationLayer();
            builder.Services.AddInfrastructureLayer(builder.Configuration);
            builder.Services.AddControllers()
                .AddJsonOptions(options => {
                    //konwersja Enumów na stringi w JSON
                    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                });
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

            app.UseMiddleware<ExceptionHandlingMiddleware>();

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
    }
}
