using ExpenseManager.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExpenseManager.API
{
    /// <summary>
    /// Represents a class that handles exceptions and converts them into ProblemDetails objects.
    /// </summary>
    /// <remarks>
    /// This class implements the IExceptionHandler interface and provides a method to handle exceptions
    /// and convert them into ProblemDetails objects. It sets the appropriate HTTP status code based on the
    /// type of exception and writes the ProblemDetails object as JSON to the response.
    /// </remarks>
    public class ExceptionToProblemDetailsHandler : IExceptionHandler
    {
        /// <summary>
        /// Tries to handle the exception and convert it into a ProblemDetails object.
        /// </summary>
        /// <param name="httpContext">The HttpContext object representing the current HTTP request and response.</param>
        /// <param name="exception">The exception to handle.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation. The task result indicates whether the exception was handled successfully.</returns>
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            // Determine the status code based on the type of exception
            var statusCode = exception switch
            {
                UserNotFoundException _ => HttpStatusCode.NotFound,
                _ => HttpStatusCode.BadRequest,
            };

            // Set the HTTP status code of the response
            httpContext.Response.StatusCode = (int)statusCode;

            // Create a ProblemDetails object with the exception details and Write the ProblemDetails object as JSON to the response
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "An error occurred",
                Detail = exception.Message,
                Type = exception.GetType().Name,
                Status = (int)statusCode
            }, cancellationToken: cancellationToken);

            return true;
        }
    }
}
