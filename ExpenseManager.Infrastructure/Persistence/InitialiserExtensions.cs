using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using ExpenseManager.Domain.Entities;

namespace ExpenseManager.Infrastructure.Persistence;

/// <summary>
/// This code snippet contains an extension method and a class for initializing and seeding a database using Entity Framework Core in a C# application.
/// </summary>
public static class InitialiserExtensions
{
    // extension method is used to initialize the database by creating a scope and getting the ApplicationDbContextInitialiser service. 
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

//responsible for initializing and seeding the database.
public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ExpenseContext _context;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ExpenseContext context)
    {
        _logger = logger;
        _context = context;
    }

    //method uses the MigrateAsync method of the Database property of the ExpenseContext to apply any pending migrations to the database.
    public async Task InitialiseAsync()
    {
        try
        {
           await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    //method calls the TrySeedAsync method, which checks if there are any existing users in the database.
    //If there are no users, it adds default users to the Users table of the ExpenseContext and saves the changes. 
    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default data
        // Seed, if necessary
        if (!_context.Users.Any())
        {
            // Add default users
            _context.Users.Add(new User { LastName = "Stark", FirstName = "Anthony", Currency= "USD" });
            _context.Users.Add(new User { LastName = "Romanova", FirstName = "Natasha", Currency= "RUB" });
            await _context.SaveChangesAsync();
        }
    }
}