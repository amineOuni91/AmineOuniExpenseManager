using ExpenseManager.Domain.Entities;
using MediatR;

namespace ExpenseManager.Application.Commands
{
    /// <summary>
    /// Represents a record that defines a command to add an expense.
    /// </summary>
    /// <param name="UserId">The ID of the user associated with the expense.</param>
    /// <param name="Date">The date of the expense.</param>
    /// <param name="Nature">The nature of the expense.</param>
    /// <param name="Amount">The amount of the expense.</param>
    /// <param name="Currency">The currency of the expense.</param>
    /// <param name="Comment">Any additional comment for the expense.</param>
    public record AddExpense(int UserId, DateTime Date, ExpenseNature Nature, decimal Amount, string Currency, string Comment) : IRequest<Expense>;
}