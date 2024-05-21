using ExpenseManager.Domain.Entities;

namespace ExpenseManager.Application.Abstractions
{
    /// <summary>
    /// Represents an interface for managing expenses in the application.
    /// </summary>
    /// <remarks>
    /// This interface provides methods for retrieving expenses by user ID, date, and amount,
    /// as well as adding a new expense to the repository.
    /// </remarks>
    public interface IExpenseRepository
    {
        Task<ICollection<Expense>> GetExpensesByUserId(int userId);
        Task<bool> GetExpensesByUserIdDateAmount(int userId, DateTime date, decimal amount);
        Task<Expense> AddExpense(Expense expense);
    }
}
