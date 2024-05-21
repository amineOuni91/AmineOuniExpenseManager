using ExpenseManager.Application.Abstractions;
using ExpenseManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Represents a repository for managing expenses in the ExpenseManager application.
    /// </summary>
    /// <remarks>
    /// This repository provides methods for adding expenses, retrieving expenses by user ID, and checking if an expense exists for a specific user, date, and amount.
    /// </remarks>
    public class ExpenseRepository(ExpenseContext context) : IExpenseRepository
    {

        /// <summary>
        /// Add a new expense to the database.
        /// </summary>
        /// <param name="expense">The expense entity to be added.</param>
        /// <returns>The added expense entity after it has been saved to the database.</returns>
        public async Task<Expense> AddExpense(Expense expense)
        {
            context.Expenses.Add(expense);

            await context.SaveChangesAsync();

            return expense;
        }

        /// <summary>
        /// Retrieves all expenses associated with a specific user ID.
        /// </summary>
        /// <param name="userId">The ID of the user whose expenses are to be retrieved.</param>
        /// <returns>A collection of expenses associated with the specified user ID.</returns>
        public async Task<ICollection<Expense>> GetExpensesByUserId(int userId)
        {
            return await context.Expenses.Include(e => e.User).Where(expense => expense.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// Checks if there is any expense matching the specified user ID, date, and amount.
        /// </summary>
        /// <param name="userId">The user ID to match against expenses.</param>
        /// <param name="date">The date to match against expenses.</param>
        /// <param name="amount">The amount to match against expenses.</param>
        /// <returns>True if at least one expense matches the specified criteria; otherwise, false.</returns>
        public async Task<bool> GetExpensesByUserIdDateAmount(int userId, DateTime date, decimal amount)
        {
            return await context.Expenses.AnyAsync(expense => expense.UserId == userId && expense.Date == date && expense.Amount == amount);
        }
    }
}
