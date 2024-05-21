using FluentAssertions;
using Moq;
using ExpenseManager.Application.CommandHandlers;
using ExpenseManager.Application.Commands;
using ExpenseManager.Application.Exceptions;
using ExpenseManager.Domain.Entities;
using FluentValidation;
using ExpenseManager.Application.Abstractions;

[TestFixture]
public class AddExpenseHandlerTests
{
    private Mock<IExpenseRepository> _mockExpenseRepository;
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IValidator<AddExpense>> _mockValidator;
    private AddExpenseHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mockExpenseRepository = new Mock<IExpenseRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockValidator = new Mock<IValidator<AddExpense>>();
        _handler = new AddExpenseHandler(_mockExpenseRepository.Object, _mockUserRepository.Object, _mockValidator.Object);
    }

    [Test]
    public async Task AddExpenseHandler_ValidInputs_ShouldAddExpense()
    {
        var addExpenseCommand = new AddExpense(1, DateTime.Now, ExpenseNature.Restaurant, 100, "USD", "Business dinner");

        _mockUserRepository.Setup(r => r.VerifyUserExistById(addExpenseCommand.UserId)).ReturnsAsync(true);
        _mockUserRepository.Setup(r => r.GetCurrencyByUserId(addExpenseCommand.UserId)).ReturnsAsync("USD");
        _mockExpenseRepository.Setup(r => r.GetExpensesByUserIdDateAmount(addExpenseCommand.UserId, addExpenseCommand.Date, addExpenseCommand.Amount))
                    .ReturnsAsync(false);
        _mockExpenseRepository.Setup(r => r.AddExpense(It.IsAny<Expense>())).ReturnsAsync(new Expense());

        var result = await _handler.Handle(addExpenseCommand, CancellationToken.None);

        result.Should().NotBeNull();
    }

    [Test]
    public void AddExpenseHandler_ShouldThrowDuplicatedExpenseException()
    {
        var addExpenseCommand = new AddExpense(1, DateTime.Now, ExpenseNature.Restaurant, 100, "USD", "Business dinner");

        _mockUserRepository.Setup(r => r.VerifyUserExistById(addExpenseCommand.UserId)).ReturnsAsync(true);
        _mockExpenseRepository.Setup(r => r.GetExpensesByUserIdDateAmount(addExpenseCommand.UserId, addExpenseCommand.Date, addExpenseCommand.Amount))
            .ReturnsAsync(true);

        _handler.Invoking(async h => await h.Handle(addExpenseCommand, CancellationToken.None))
                            .Should().ThrowAsync<DuplicatedExpenseException>()
                            .WithMessage("Duplicate expense detected");
    }

    [Test]
    public void AddExpenseHandler_ShouldThrowNotSameCurrencyException()
    {
        var addExpenseCommand = new AddExpense(1, DateTime.Now, ExpenseNature.Restaurant, 100, "USD", "Business dinner");

        _mockUserRepository.Setup(r => r.VerifyUserExistById(addExpenseCommand.UserId)).ReturnsAsync(true);
        _mockExpenseRepository.Setup(r => r.GetExpensesByUserIdDateAmount(addExpenseCommand.UserId, addExpenseCommand.Date, addExpenseCommand.Amount))
            .ReturnsAsync(false);
        _mockUserRepository.Setup(r => r.GetCurrencyByUserId(addExpenseCommand.UserId))
            .ReturnsAsync("EUR");

        _handler.Invoking(async h => await h.Handle(addExpenseCommand, CancellationToken.None))
                    .Should().ThrowAsync<NotSameCurrencyException>()
                    .WithMessage("User did not have same expense currency");
    }

    [Test]
    public void AddExpenseHandler_NonExistentUser_ShouldThrowUserNotFoundException()
    {
        var addExpenseCommand = new AddExpense(999, DateTime.Now, ExpenseNature.Restaurant, 100, "USD", "Business dinner");

        _mockUserRepository.Setup(r => r.VerifyUserExistById(addExpenseCommand.UserId)).ReturnsAsync(false);

        _handler.Invoking(async h => await h.Handle(addExpenseCommand, CancellationToken.None))
            .Should().ThrowAsync<UserNotFoundException>()
            .WithMessage("User not found");
    }
}