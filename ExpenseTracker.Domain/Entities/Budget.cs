using ExpenseTracker.Domain.Common;

public class Budget : BaseEntity
{
    public decimal Amount { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public int UserId { get; set; }
}