using ExpenseManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseManager.Infrastructure.Configuration;

/// <summary>
/// UserConfiguration implements the IEntityTypeConfiguration interface.
/// It is used to configure the entity mapping for the User class in the ExpenseManager.Domain.Entities namespace.
/// The Configure method is overridden to define the relationship between the User and Expense entities using the Entity Framework Core.
/// It specifies that a User can have many Expenses, and each Expense belongs to a User.
/// The foreign key property UserId is used to establish the relationship between the entities.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(u => u.Expenses)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId);
    }
}
