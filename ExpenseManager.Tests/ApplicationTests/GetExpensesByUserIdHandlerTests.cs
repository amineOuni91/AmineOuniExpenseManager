using FluentAssertions;
using ExpenseManager.Application.QueryHandlers;
using ExpenseManager.Application.Queries;
using ExpenseManager.Application.Queries.Enums;
using ExpenseManager.Domain.Entities;
using Moq;
using ExpenseManager.Application.Abstractions;

namespace ExpenseManager.Tests
{
    [TestFixture]
    public class GetExpensesByUserIdHandlerTests
    {
        private Mock<IExpenseRepository> _mockRepository;
        private GetExpensesByUserIdHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IExpenseRepository>();
            _handler = new GetExpensesByUserIdHandler(_mockRepository.Object);
        }

        [Test]
        public async Task Handle_ReturnsTransformedAndSortedExpensesByUserId()
        {
            // Arrange
            var userId = 1;
            var expenses = new List<Expense>
            {
                new Expense { Id = 1, User = new User { FirstName = "John", LastName = "Doe" }, Date = new System.DateTime(2021, 1, 1), Nature = ExpenseNature.Restaurant, Amount = 100, Currency = "USD", Comment = "Lunch" },
                new Expense { Id = 2, User = new User { FirstName = "Jane", LastName = "Doe" }, Date = new System.DateTime(2021, 1, 2), Nature = ExpenseNature.Hotel, Amount = 200, Currency = "USD", Comment = "Dinner" }
            };
            _mockRepository.Setup(r => r.GetExpensesByUserId(userId)).ReturnsAsync(expenses);

            var request = new GetExpensesByUserId (userId, SortedBy.Date);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            result.First().UserName.Should().Be("John Doe");
            result.Last().UserName.Should().Be("Jane Doe");
            result.First().Amount.Should().Be(100);
            result.Last().Amount.Should().Be(200);
        }

        [Test]
        public async Task Handle_SortsExpensesByAmountWhenRequested()
        {
            // Arrange
            var userId = 1;
            var expenses = new List<Expense>
            {
                new Expense { Id = 1, User = new User { FirstName = "John", LastName = "Doe" }, Date = new System.DateTime(2021, 1, 1), Nature = ExpenseNature.Restaurant, Amount = 200, Currency = "USD", Comment = "Lunch" },
                new Expense { Id = 2, User = new User { FirstName = "Jane", LastName = "Doe" }, Date = new System.DateTime(2021, 1, 2), Nature = ExpenseNature.Hotel, Amount = 100, Currency = "USD", Comment = "Dinner" }
            };
            _mockRepository.Setup(r => r.GetExpensesByUserId(userId)).ReturnsAsync(expenses);

            var request = new GetExpensesByUserId (userId, SortedBy.Amount);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            result.First().Amount.Should().Be(100);
            result.Last().Amount.Should().Be(200);
        }

        [Test]
        public async Task Handle_ReturnsEmptyWhenNoExpensesFound()
        {
            // Arrange
            var userId = 1;
            _mockRepository.Setup(r => r.GetExpensesByUserId(userId)).ReturnsAsync(new List<Expense>());

            var request = new GetExpensesByUserId(userId, SortedBy.Date);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }
    }
}