using ExpenseManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace ExpenseManager.Infrastructure.Persistence
{
    /// <summary>
    /// Represents a class that defines the ExpenseContext for managing expenses and users in the database.
    /// </summary>
    /// <remarks>
    /// This class inherits from the DbContext class and provides access to the Expense and User entities through DbSet properties.
    /// It also overrides the OnModelCreating method to apply configurations from the executing assembly.
    /// </remarks>
    public class ExpenseContext : DbContext
    {
        public ExpenseContext(DbContextOptions<ExpenseContext> options)
            : base(options)
        {
        }

        public ExpenseContext() { }

        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
