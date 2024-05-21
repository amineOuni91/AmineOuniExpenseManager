using ExpenseManager.Application.Commands;
using ExpenseManager.Application.Queries;
using ExpenseManager.Application.Queries.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Adds an expense.
        /// </summary>
        /// <param name="command">The command containing the expense details.</param>
        /// <returns>The result of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> AddExpense(AddExpense command)
        {
            return Ok(await mediator.Send(command));
        }

        /// <summary>
        /// Retrieves expenses by user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="sortedBy">The sorting option for the expenses.</param>
        /// <returns>The expenses for the specified user ID.</returns>
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetExpensesByUserId(int userId, [FromQuery] SortedBy SortedBy)
        {
            return Ok(await mediator.Send(new GetExpensesByUserId(userId, SortedBy)));
        }
    }
}
