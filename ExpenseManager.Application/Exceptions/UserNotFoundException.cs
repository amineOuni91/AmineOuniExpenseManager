using Serilog;

namespace ExpenseManager.Application.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a user is not found.
    /// </summary>
    /// <remarks>
    /// This exception is typically thrown when a user cannot be found in the database.
    /// </remarks>
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
        {
        }

        public UserNotFoundException(string message)
            : base(message)
        {
            Log.Warning(message);
        }
    }
}
