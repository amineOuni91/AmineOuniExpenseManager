using ExpenseManager.Application.Queries.Enums;
using ExpenseManager.Application.Queries.Responses;
using MediatR;

namespace ExpenseManager.Application.Queries
{
    /// <summary>
    /// Represents a C# record that defines a query to retrieve expenses by user ID and sorted by a specified criteria.
    /// </summary>
    /// <param name="UserId">The ID of the user for which to retrieve expenses.</param>
    /// <param name="SortedBy">The criteria by which to sort the expenses.</param>
    /// <returns>A collection of ExpenseDto objects representing the retrieved expenses.</returns>
    /// <remarks>
    /// This record is used as a request in the MediatR framework to query the expenses by user ID and sorted by a specified criteria.
    /// </remarks>
    public record GetExpensesByUserId(int UserId, SortedBy SortedBy) : IRequest<ICollection<ExpenseDto>>;
}
