using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Entities;

public class PaymentMethod : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public int UserId { get; set; }

    public ICollection<FinancialTransaction> Transactions { get; set; }
        = new List<FinancialTransaction>();
}