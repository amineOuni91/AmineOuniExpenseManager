using FluentAssertions;
using ExpenseManager.Infrastructure.Persistence.Repositories;
using ExpenseManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ExpenseManager.Infrastructure.Persistence;

namespace ExpenseManager.Tests
{
    [TestFixture]
    public class ExpenseRepositoryTests
    {
        private ExpenseContext _context;
        private ExpenseRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ExpenseContext>()
                .UseInMemoryDatabase(databaseName: "ExpenseManagerTestDb")
                .Options;
            _context = new ExpenseContext(options);
            _repository = new ExpenseRepository(_context);

            // Ensure the database is clean before each test
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Users.AddRange(
        new User { Id = 1, FirstName = "John", LastName = "Wick", Currency = "USD" },
        new User { Id = 2, FirstName = "Jane", LastName = "Doe", Currency = "USD" }
    );
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task TestAddExpenseReturnsExpense()
        {
            var expense = new Expense { UserId = 1, Amount = 100, Date = DateTime.Now, Comment = "comment", Currency = "USD", Nature = ExpenseNature.Restaurant };

            var result = await _repository.AddExpense(expense);

            result.Should().BeEquivalentTo(expense);
            _context.Expenses.Should().ContainSingle(e => e == result);
        }

        [Test]
        public async Task TestGetExpensesByUserIdReturnsCorrectExpenses()
        {
            _context.Expenses.AddRange(
                new Expense { UserId = 1, Amount = 50, Date = DateTime.UtcNow, Comment = "comment", Currency = "USD", Nature = ExpenseNature.Restaurant },
                new Expense { UserId = 1, Amount = 150, Date = DateTime.UtcNow.AddDays(-1), Comment = "comment", Currency = "USD", Nature = ExpenseNature.Restaurant },
                new Expense { UserId = 2, Amount = 200, Date = DateTime.UtcNow, Comment = "comment", Currency = "USD", Nature = ExpenseNature.Restaurant }
            );
            await _context.SaveChangesAsync();

            var result = await _repository.GetExpensesByUserId(1);

            result.Should().HaveCount(2).And.OnlyContain(e => e.UserId == 1);
        }

        [Test]
        public async Task TestGetExpensesByUserIdDateAmountReturnsTrueIfMatchExists()
        {
            _context.Expenses.Add(new Expense { UserId = 1, Amount = 100, Date = new DateTime(2024, 5, 6), Comment = "comment", Currency = "USD", Nature = ExpenseNature.Restaurant });
            await _context.SaveChangesAsync();

            var result = await _repository.GetExpensesByUserIdDateAmount(1, new DateTime(2024, 5, 6), 100);

            result.Should().BeTrue();
        }
    }
}