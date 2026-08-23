using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;

public class FinancialTransaction : BaseEntity
{
    public decimal Amount { get; set; }

    public TransactionType Type { get; set; }

    public DateTime TransactionDate { get; set; }

    public string? Note { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; set; } = default!;

    public int PaymentMethodId { get; set; }

    public PaymentMethod PaymentMethod { get; set; } = default!;

    public int UserId { get; set; }
}