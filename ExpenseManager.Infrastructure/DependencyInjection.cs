using Ardalis.GuardClauses;
using ExpenseManager.Application.Abstractions;
using ExpenseManager.Infrastructure.Persistence;
using ExpenseManager.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace ExpenseManager.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// extension method for IServiceCollection that adds infrastructure services to the collection.
    /// It configures the ExpenseContext with the provided connection string and registers the necessary repositories.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the services to.</param>
    /// <param name="configuration">The IConfiguration instance containing the connection string.</param>
    /// <returns>The modified IServiceCollection.</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ExpenseDb");

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ExpenseContext>((sp, options) =>
        {
            options.UseSqlite(connectionString);
        });

        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
