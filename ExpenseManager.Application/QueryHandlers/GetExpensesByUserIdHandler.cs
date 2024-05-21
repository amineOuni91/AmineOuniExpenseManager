using ExpenseManager.Application.Abstractions;
using ExpenseManager.Application.Commands;
using ExpenseManager.Application.Queries;
using ExpenseManager.Application.Queries.Enums;
using ExpenseManager.Application.Queries.Responses;
using ExpenseManager.Domain.Entities;
using MediatR;

namespace ExpenseManager.Application.QueryHandlers
{
    /// <summary>
    /// This classit is a query handler that implements the IRequestHandler interface. The class is responsible for handling the GetExpensesByUserId query and returning a collection of ExpenseDto objects.
    /// </summary>
    /// <typeparam name="IExpenseRepository">The interface representing the expense repository.</typeparam>
    /// <typeparam name="GetExpensesByUserId">The query class representing the GetExpensesByUserId query.</typeparam>
    /// <typeparam name="ICollection<ExpenseDto>">The collection of ExpenseDto objects returned by the query handler.</typeparam>
    /// <returns>A collection of ExpenseDto objects based on the provided query parameters.</returns>
    public class GetExpensesByUserIdHandler(IExpenseRepository expenseRepository) : IRequestHandler<GetExpensesByUserId, ICollection<ExpenseDto>>
    {
        public async Task<ICollection<ExpenseDto>> Handle(GetExpensesByUserId request, CancellationToken cancellationToken)
        {
            var expenses = (await expenseRepository.GetExpensesByUserId(request.UserId)).Select(e => new ExpenseDto
            {
                Id = e.Id,
                UserName = $"{e.User.FirstName} {e.User.LastName}",
                Date = e.Date,
                Nature = Enum.GetName(typeof(ExpenseNature), e.Nature)!,
                Amount = e.Amount,
                Currency = e.Currency,
                Comment = e.Comment
            });

            if (request.SortedBy == SortedBy.Amount)
            {
                return expenses.OrderBy(e => e.Amount).ToList();
            }
            else
            {
                return expenses.OrderBy(e => e.Date).ToList();
            }
             
        }
    }
}
