namespace LibrarySystem;

public class LibraryItem
{
    public string CatalogNumber { get; }
    public string Title { get; }
    public decimal BaseLateFee { get; private set; }
    public bool IsWithdrawn { get; private set; }
    public bool IsOnLoan { get; private set; }

    public int LoanPeriodDays { get; }
    public decimal LateFeeMultiplier { get; }

    protected LibraryItem(string catalogNumber, string title, decimal baseLateFee, int loanPeriodDays, decimal lateFeeMultiplier)
    {
        if (string.IsNullOrWhiteSpace(catalogNumber)) throw new ArgumentException("Catalog number cannot be empty.");
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title cannot be empty.");
        if (baseLateFee <= 0) throw new ArgumentException("Base late fee must be positive.");

        CatalogNumber = catalogNumber;
        Title = title;
        BaseLateFee = baseLateFee;
        LoanPeriodDays = loanPeriodDays;
        LateFeeMultiplier = lateFeeMultiplier;
    }

    public void SetBaseLateFee(decimal newFee)
    {
        if (newFee <= 0) throw new ArgumentException("New late fee must be greater than zero.");
        BaseLateFee = newFee;
    }

    public void Withdraw() => IsWithdrawn = true;
    public void Restore() => IsWithdrawn = false;

    internal void SetOnLoan(bool isOnLoan) => IsOnLoan = isOnLoan;

    public decimal DailyLateFee() => BaseLateFee * LateFeeMultiplier;
}

public class Book : LibraryItem
{
    public Book(string catalogNumber, string title, decimal baseLateFee)
        : base(catalogNumber, title, baseLateFee, loanPeriodDays: 21, lateFeeMultiplier: 1.0m)
    {
    }
}

public class DVD : LibraryItem
{
    public DVD(string catalogNumber, string title, decimal baseLateFee)
        : base(catalogNumber, title, baseLateFee, loanPeriodDays: 7, lateFeeMultiplier: 2.0m)
    {
    }
}

public class Magazine : LibraryItem
{
    public Magazine(string catalogNumber, string title, decimal baseLateFee)
        : base(catalogNumber, title, baseLateFee, loanPeriodDays: 3, lateFeeMultiplier: 0.5m)
    {
    }
}