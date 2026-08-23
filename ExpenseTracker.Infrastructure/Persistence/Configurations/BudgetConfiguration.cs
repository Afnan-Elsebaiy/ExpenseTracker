using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.Infrastructure.Persistence.Configurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.ToTable("Budgets");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Amount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(b => b.Month)
               .IsRequired();

        builder.Property(b => b.Year)
               .IsRequired();

        builder.Property(b => b.UserId)
               .IsRequired();

        builder.HasIndex(b => new
        {
            b.UserId,
            b.Month,
            b.Year
        }).IsUnique();
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}