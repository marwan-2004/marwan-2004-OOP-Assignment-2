using LibrarySystem;

Console.WriteLine("=========================================\n");
Console.WriteLine(" Library System — Part 03 Proof");
Console.WriteLine("=========================================\n");

// --- 1. Lines that MUST NOT COMPILE (Uncomment to test compilation failures) ---
// Person p = new Person("P1", "Ali", "0100");                     // MUST NOT COMPILE (Protected constructor)
// Member m = new Member("M1", "Sara", "0111", 5, 0);             // MUST NOT COMPILE (Protected constructor)
// Staff s = new Staff("S1", "Mona", "0122", DateOnly.MinValue, 5000); // MUST NOT COMPILE (Protected constructor)
// LibraryItem item = new LibraryItem("I1", "Book", 10, 7, 1);    // MUST NOT COMPILE (Protected constructor)
// student.FullName = "New Name";                                 // MUST NOT COMPILE (Get-only property)
// student.Loans.Add(new Loan(...));                             // MUST NOT COMPILE (IReadOnlyList has no Add)
// item.IsOnLoan = true;                                         // MUST NOT COMPILE (Set internal/private)

// --- 2. Polymorphic Staff Salaries ---
Console.WriteLine("--- Staff Salaries ---");
List<Staff> staffList = new List<Staff>
{
    new Librarian("ST-01", "Hoda", "010111", new DateOnly(2022, 1, 1), 6000m),
    new HeadLibrarian("ST-02", "Maged", "010222", new DateOnly(2020, 5, 10), 9000m),
    new Shelver("ST-03", "Kareem", "010333", new DateOnly(2023, 3, 15), 4500m, "Fiction")
};

foreach (var staff in staffList)
{
    Console.WriteLine($"{staff.FullName} ({staff.GetType().Name}): Monthly Pay = {staff.GetMonthlyPay():C}");
}

Console.WriteLine("\n--- Library Items ---");
List<LibraryItem> items = new List<LibraryItem>
{
    new Book("BK-101", "C# in Depth", 5.0m),
    new DVD("DVD-202", "OOP Paradigm Video", 10.0m),
    new Magazine("MAG-303", "Tech Monthly", 4.0m)
};

foreach (var item in items)
{
    Console.WriteLine($"{item.Title} ({item.GetType().Name}): Loan Period = {item.LoanPeriodDays} days | Daily Late Fee = {item.DailyLateFee():C}");
}

Console.WriteLine("\n--- Premium Member Loan Test ---");
var premiumMember = new PremiumMember("M-100", "Marwan", "010999", feeDiscountPercentage: 20m);
var dvd = new DVD("DVD-555", "Design Patterns Animated", 10m);

var borrowDate = new DateOnly(2026, 10, 1);
var loan = premiumMember.BorrowItem("LN-1", dvd, borrowDate);
Console.WriteLine($"Borrowed: {dvd.Title} on {borrowDate:yyyy-MM-dd}. Due Date: {loan.DueDate:yyyy-MM-dd}");

var returnDate = new DateOnly(2026, 10, 13);
premiumMember.ReturnLoan(loan, returnDate);

Console.WriteLine($"Returned on: {returnDate:yyyy-MM-dd}");
Console.WriteLine($"Late Fee Charged (with 20% discount): {loan.LateFee:C}");
Console.WriteLine($"Premium Reading Points Total: {premiumMember.ReadingPoints}");

Console.WriteLine("\n--- Rule Validation Exceptions ---");

try
{
    var headLibrarian = (HeadLibrarian)staffList[1];
    var book = new Book("BK-999", "Withdrawn Title", 5m);
    headLibrarian.WithdrawItem(book);
    premiumMember.BorrowItem("LN-2", book, DateOnly.FromDateTime(DateTime.Now));
}
catch (Exception ex)
{
    Console.WriteLine($"[Expected Error - Withdrawn]: {ex.Message}");
}

try
{
    var student1 = new StudentMember("S-1", "Ali", "01111");
    var student2 = new StudentMember("S-2", "Omar", "02222");
    var book = new Book("BK-111", "Clean Code", 5m);

    student1.BorrowItem("LN-3", book, DateOnly.FromDateTime(DateTime.Now));
    student2.BorrowItem("LN-4", book, DateOnly.FromDateTime(DateTime.Now));
}
catch (Exception ex)
{
    Console.WriteLine($"[Expected Error - Double Loan]: {ex.Message}");
}

try
{
    var student = new StudentMember("S-3", "Nora", "03333");
    student.BorrowItem("LN-10", new Book("B1", "Book 1", 5m), DateOnly.FromDateTime(DateTime.Now));
    student.BorrowItem("LN-11", new Book("B2", "Book 2", 5m), DateOnly.FromDateTime(DateTime.Now));
    student.BorrowItem("LN-12", new Book("B3", "Book 3", 5m), DateOnly.FromDateTime(DateTime.Now));
    student.BorrowItem("LN-13", new Book("B4", "Book 4", 5m), DateOnly.FromDateTime(DateTime.Now));
}
catch (Exception ex)
{
    Console.WriteLine($"[Expected Error - Student Max Loans]: {ex.Message}");
}

try
{
    var student = new StudentMember("S-4", "Mostafa", "04444");
    var book = new Book("B5", "Book 5", 5m);
    var testLoan = student.BorrowItem("LN-20", book, new DateOnly(2026, 10, 1));

    student.ReturnLoan(testLoan, new DateOnly(2026, 10, 5));
    student.ReturnLoan(testLoan, new DateOnly(2026, 10, 6));
}
catch (Exception ex)
{
    Console.WriteLine($"[Expected Error - Double Return]: {ex.Message}");
}

try
{
    var student = new StudentMember("S-5", "Youssef", "05555");
    var book = new Book("B6", "Book 6", 5m);
    var testLoan = student.BorrowItem("LN-21", book, new DateOnly(2026, 10, 1));

    student.ReturnLoan(testLoan, new DateOnly(2026, 10, 5));
    testLoan.MarkAsLost();
}
catch (Exception ex)
{
    Console.WriteLine($"[Expected Error - Mark Returned as Lost]: {ex.Message}");
}

Console.WriteLine("\nAll inheritance rules successfully proven!");