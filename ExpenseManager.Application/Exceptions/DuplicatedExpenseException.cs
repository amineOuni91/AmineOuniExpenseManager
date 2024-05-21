using Serilog;

namespace ExpenseManager.Application.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a duplicated expense is encountered.
    /// </summary>
    public class DuplicatedExpenseException : Exception
    {
        public DuplicatedExpenseException()
        {
        }

        public DuplicatedExpenseException(string message)
            : base(message)
        {
            Log.Warning(message);
        }
    }
}
