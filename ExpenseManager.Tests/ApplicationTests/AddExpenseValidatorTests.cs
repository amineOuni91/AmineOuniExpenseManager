using FluentAssertions;
using ExpenseManager.Application.Commands;
using ExpenseManager.Domain.Entities;

namespace ExpenseManager.Tests
{
    [TestFixture]
    public class AddExpenseValidatorTests
    {
        private AddExpenseValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new AddExpenseValidator();
        }

        [Test]
        public void TestExpenseWithEmptyComment()
        {
            var expense = new AddExpense(1, DateTime.Now, ExpenseNature.Restaurant, 1, "USD", "");
            var result = validator.Validate(expense);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == "The comment can't be empty.");
        }

        [Test]
        public void AddExpenseValidator_Should_Reject_Date_Outside_Three_Months()
        {
            var expense = new AddExpense(1, DateTime.UtcNow.AddMonths(-4), ExpenseNature.Restaurant, 1, "USD", "Old expense");

            var result = validator.Validate(expense);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage.Contains("The expense date"));
        }

        [Test]
        public void AddExpenseValidator_Should_Reject_Invalid_Currency()
        {
            var expense = new AddExpense(1, DateTime.UtcNow, ExpenseNature.Misc, 1, "XYZ", "Conference fee");
            var result = validator.Validate(expense);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage.Contains("The currency XYZ is invalid"));
        }

        [Test]
        public void AddExpenseValidator_Should_Reject_Invalid_Nature()
        {
            var expense = new AddExpense(1, DateTime.UtcNow, (ExpenseNature)999, 1, "USD", "Conference fee");
            var result = validator.Validate(expense);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage.Contains("The Nature is invalid, it must be Restaurant = 0, Hotel = 1 or Misc = 2"));
        }

        [Test]
        public void AddExpenseValidator_Should_Reject_Invalid_Amount()
        {
            var expense = new AddExpense(1, DateTime.UtcNow, ExpenseNature.Restaurant, -50.00m, "USD", "Lunch meeting");
            var result = validator.Validate(expense);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == "The amount must be greater than zero.");
        }

        [Test]
        public void AddExpenseValidator_Should_Validate_NonEmpty_Comment()
        {
            var expense = new AddExpense(1, DateTime.UtcNow, ExpenseNature.Restaurant, 1, "USD", "Lunch meeting");
            var result = validator.Validate(expense);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().NotContain(e => e.ErrorMessage == "The comment can't be empty.");
        }

        [Test]
        public void AddExpenseValidator_Should_Validate_Date_Within_Three_Months()
        {
            var expense = new AddExpense(1, DateTime.UtcNow.AddDays(-20), ExpenseNature.Hotel, 1, "EUR", "Hotel booking");
            var result = validator.Validate(expense);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().NotContain(e => e.ErrorMessage.Contains("The expense date is invalid"));
        }

        [Test]
        public void AddExpenseValidator_Should_Validate_Valid_Currency()
        {
            var expense = new AddExpense(1, DateTime.UtcNow, ExpenseNature.Misc, 1, "JPY", "Miscellaneous expenses");
            var result = validator.Validate(expense);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().NotContain(e => e.ErrorMessage.Contains("The currency is invalid"));
        }

        [Test]
        public void AddExpenseValidator_Should_Validate_Valid_Nature()
        {
            var expense = new AddExpense(1, DateTime.UtcNow, ExpenseNature.Hotel, 1, "USD", "Conference fee");
            var result = validator.Validate(expense);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().NotContain(e => e.ErrorMessage.Contains("The Nature is invalid"));
        }

        [Test]
        public void AddExpenseValidator_Should_Validate_Valid_Amount()
        {
            var expense = new AddExpense(1, DateTime.UtcNow, ExpenseNature.Restaurant, 100.50m, "USD", "Lunch meeting");
            var result = validator.Validate(expense);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().NotContain(e => e.ErrorMessage == "The amount must be greater than zero.");
        }
    }
}