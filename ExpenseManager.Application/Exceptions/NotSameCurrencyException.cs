using Serilog;

namespace ExpenseManager.Application.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when two currencies are not the same.
    /// </summary>
    /// <remarks>
    /// This exception is typically thrown when performing operations that require the user currency and expense currency to be the same.
    /// </remarks>
    public class NotSameCurrencyException : Exception
    {
        public NotSameCurrencyException()
        {
        }

        public NotSameCurrencyException(string message)
            : base(message)
        {
            Log.Warning(message);
        }
    }
}
