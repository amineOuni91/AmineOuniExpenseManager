namespace ExpenseManager.Domain.Entities;

/// <summary>
/// Represents an expense entity.
/// </summary>
/// <remarks>
/// An expense has an ID, user ID, date, nature, amount, currency, comment, and a reference to the user it belongs to.
/// </remarks>
public class Expense
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public ExpenseNature Nature { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Comment { get; set; }
    public virtual User User { get; set; }
}
