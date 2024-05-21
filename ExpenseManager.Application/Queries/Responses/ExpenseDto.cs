namespace ExpenseManager.Application.Queries.Responses
{
    /// <summary>
    /// Represents a data transfer object for an expense.
    /// </summary>
    /// <remarks>
    /// This class contains properties for the ID, date, nature, amount, currency, comment, and user name of an expense.
    /// </remarks>
    public class ExpenseDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Nature { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
    }
}
