using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetCare.Application.Behaviors;
using System.Reflection;

namespace PetCare.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            var assembly = Assembly.GetAssembly(typeof(ServiceCollectionExtensions));

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly!);

                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
