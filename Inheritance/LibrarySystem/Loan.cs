namespace LibrarySystem;

public class Loan
{
    public string LoanId { get; }
    public DateOnly BorrowDate { get; }
    public DateOnly? ReturnDate { get; private set; }
    public Member BorrowingMember { get; }
    public LibraryItem Item { get; }
    public LoanStatus Status { get; private set; }

    public DateOnly DueDate => BorrowDate.AddDays(Item.LoanPeriodDays);

    public decimal LateFee
    {
        get
        {
            if (Status != LoanStatus.Returned || ReturnDate == null || ReturnDate <= DueDate)
                return 0m;

            int daysLate = ReturnDate.Value.DayNumber - DueDate.DayNumber;
            decimal baseFee = daysLate * Item.DailyLateFee();
            decimal discount = baseFee * (BorrowingMember.FeeDiscountPercentage / 100m);
            return Math.Max(0m, baseFee - discount);
        }
    }

    public Loan(string loanId, DateOnly borrowDate, Member borrowingMember, LibraryItem item)
    {
        if (string.IsNullOrWhiteSpace(loanId)) throw new ArgumentException("Loan ID cannot be empty.");

        LoanId = loanId;
        BorrowDate = borrowDate;
        BorrowingMember = borrowingMember;
        Item = item;
        Status = LoanStatus.Borrowed;
    }

    public void CompleteReturn(DateOnly returnDate)
    {
        if (returnDate < BorrowDate) throw new ArgumentException("Return date cannot be earlier than borrow date.");
        if (Status == LoanStatus.Returned) throw new InvalidOperationException("Loan is already returned.");
        if (Status == LoanStatus.Lost) throw new InvalidOperationException("Cannot return a loan marked as lost.");

        ReturnDate = returnDate;
        Status = LoanStatus.Returned;
        Item.SetOnLoan(false);

        if (BorrowingMember is PremiumMember premium)
        {
            premium.AddPoints(5);
        }
    }

    public void MarkAsLost()
    {
        if (Status == LoanStatus.Returned) throw new InvalidOperationException("Cannot mark a returned loan as lost.");

        Status = LoanStatus.Lost;
        Item.SetOnLoan(false);
    }
}