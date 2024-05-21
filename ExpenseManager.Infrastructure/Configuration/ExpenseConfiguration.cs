using ExpenseManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseManager.Infrastructure.Configuration;

/// <summary>
/// This code represents the configuration class for the Expense entity in the ExpenseManager.Domain.Entities namespace.
/// It implements the IEntityTypeConfiguration interface and is used to configure the Expense entity in the Entity Framework Core.
/// The Configure method is empty and can be overridden to provide custom configuration for the Expense entity.
/// </summary>
public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
    }
}
