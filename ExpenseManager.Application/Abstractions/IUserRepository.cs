namespace ExpenseManager.Application.Abstractions
{
    /// <summary>
    /// Represents an interface for a user repository.
    /// </summary>
    /// <remarks>
    /// This interface provides methods for retrieving the currency associated with a user ID and verifying the existence of a user by ID.
    /// </remarks>
    public interface IUserRepository
    {
        Task<string> GetCurrencyByUserId(int userId);
        Task<bool> VerifyUserExistById(int userId);


    }
}
