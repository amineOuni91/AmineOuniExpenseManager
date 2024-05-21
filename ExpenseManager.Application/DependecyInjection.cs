using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseManager.Application
{
    /// <summary>
    /// This code is an extension method for IServiceCollection. 
    /// It adds application services to the service collection by registering MediatR services and validators from the assembly of the DependencyInjection class.
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(configuration =>
                configuration.RegisterServicesFromAssembly(assembly));

            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
