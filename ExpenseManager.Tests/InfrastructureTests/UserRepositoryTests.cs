using ExpenseManager.Domain.Entities;
using ExpenseManager.Infrastructure.Persistence;
using ExpenseManager.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.Tests
{
    public class UserRepositoryTests
    {
        private DbContextOptions<ExpenseContext> _dbContextOptions;


        [SetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ExpenseContext>()
                .UseInMemoryDatabase(databaseName: "ExpenseManagerTestDb")
                .Options;

            using (var context = new ExpenseContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Users.Add(new User { Id = 1, Currency = "USD", FirstName = "Amine", LastName = "Ouni"  });
                context.Users.Add(new User { Id = 2, Currency = "EUR", FirstName = "John", LastName = "Wick" });
                context.SaveChanges();
            }
        }

        [Test]
        public async Task GetCurrencyByUserId_ShouldReturnCorrectCurrency()
        {
            using (var context = new ExpenseContext(_dbContextOptions))
            {
                var userRepository = new UserRepository(context);
                var currency = await userRepository.GetCurrencyByUserId(1);
                currency.Should().Be("USD");
            }
        }

        [Test]
        public async Task VerifyUserExistById_ShouldReturnTrueIfUserExists()
        {
            using (var context = new ExpenseContext(_dbContextOptions))
            {
                var userRepository = new UserRepository(context);
                var exists = await userRepository.VerifyUserExistById(1);
                exists.Should().BeTrue();
            }
        }
    }
}