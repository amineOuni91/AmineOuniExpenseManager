using ExpenseManager.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Represents a repository for managing user data in the ExpenseManager application.
    /// </summary>
    /// <remarks>
    /// This repository provides methods for retrieving currency information and verifying the existence of a user by their ID.
    /// </remarks>
    /// <typeparam name="ExpenseContext">The database context for accessing user data.</typeparam>
    public class UserRepository(ExpenseContext context) : IUserRepository
    {
        /// <summary>
        /// Retrieves the currency associated with a user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The currency associated with the user.</returns>
        public async Task<string> GetCurrencyByUserId(int userId)
        {
            return (await context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == userId)).Currency;
        }

        /// <summary>
        /// Verifies the existence of a user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>True if the user exists, otherwise false.</returns
        public async Task<bool> VerifyUserExistById(int userId)
        {
            return await context.Users.AnyAsync(user => user.Id == userId);
        }
    }
}
