using FluentValidation;
using System.Globalization;

namespace ExpenseManager.Application.Commands
{
    /// <summary>
    /// Validator class for the AddExpense command.
    /// </summary>
    /// <remarks>
    /// This class is responsible for validating the properties of the AddExpense command.
    /// It ensures that the comment is not empty, the amount greater tha zero, the date is within the last three months, the nature is valid, and the currency is valid.
    /// </remarks>
    public sealed class AddExpenseValidator : AbstractValidator<AddExpense>
    {
        public AddExpenseValidator()
        {
            RuleFor(expense => expense.Comment)
                .NotEmpty()
                .WithMessage("The comment can't be empty.");

            RuleFor(expense => expense.Amount)
                .GreaterThan(0)
                .WithMessage("The amount must be greater than zero.");

            RuleFor(expense => expense.Date)
                .Must(BeValidDate)
                .WithMessage((expense, date) => $"The expense date {date} is invalid");

            RuleFor(expense => expense.Nature)
            .IsInEnum()
            .WithMessage($"The {nameof(AddExpense.Nature)} is invalid, it must be Restaurant = 0, Hotel = 1 or Misc = 2");

            RuleFor(expense => expense.Currency)
                .Must(BeValidCurrency)
                .WithMessage((expense, currency) => $"The currency {currency} is invalid");
        }

        private const int MaxMonthsAgo = 3;

        private bool BeValidDate(DateTime date)
        {
            DateTime threeMonthsAgo = DateTime.UtcNow.AddMonths(-MaxMonthsAgo);
            return date <= DateTime.UtcNow && date >= threeMonthsAgo;
        }

        private bool BeValidCurrency(string currency)
        {
            return CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Any(culture => new RegionInfo(culture.Name).ISOCurrencySymbol == currency);
        }
    }
}