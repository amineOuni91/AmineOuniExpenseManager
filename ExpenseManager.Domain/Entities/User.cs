namespace ExpenseManager.Domain.Entities;

/// <summary>
/// Represents a user in the Expense Manager system.
/// </summary>
/// <remarks>
/// The User class contains properties for the user's ID, first name, last name, currency, and a collection of expenses.
/// </remarks>
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Currency { get; set; }
    public virtual ICollection<Expense> Expenses { get; set; }
}
