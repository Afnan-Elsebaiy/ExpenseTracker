using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.Infrastructure.Persistence.Configurations;

public class FinancialTransactionConfiguration : IEntityTypeConfiguration<FinancialTransaction>
{
    public void Configure(EntityTypeBuilder<FinancialTransaction> builder)
    {
        builder.ToTable("FinancialTransactions");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Amount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(t => t.Type)
               .IsRequired();

        builder.Property(t => t.TransactionDate)
               .IsRequired();

        builder.Property(t => t.Note)
               .HasMaxLength(500);

        builder.Property(t => t.UserId)
               .IsRequired();

        builder.HasOne(t => t.Category)
               .WithMany(c => c.Transactions)
               .HasForeignKey(t => t.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.PaymentMethod)
               .WithMany(p => p.Transactions)
               .HasForeignKey(t => t.PaymentMethodId)
               .OnDelete(DeleteBehavior.Restrict);
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}