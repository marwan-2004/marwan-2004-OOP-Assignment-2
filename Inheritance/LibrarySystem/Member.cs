using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem
{
    public class Member : Person
    {
        private readonly List<Loan> _loans = new();

        public int MaxLoans { get; }
        public decimal FeeDiscountPercentage { get; }
        public IReadOnlyList<Loan> Loans => _loans;

        protected Member(string personId, string fullName, string phone, int maxLoans, decimal feeDiscountPercentage)
            : base(personId, fullName, phone)
        {
            MaxLoans = maxLoans;
            FeeDiscountPercentage = feeDiscountPercentage;
        }

        public int ActiveLoanCount()
        {
            int count = 0;
            foreach (var loan in _loans)
            {
                if (loan.Status == LoanStatus.Borrowed)
                {
                    count++;
                }
            }
            return count;
        }

        public Loan BorrowItem(string loanId, LibraryItem item, DateOnly borrowDate)
        {
            if (item.IsWithdrawn) throw new InvalidOperationException("Withdrawn items cannot be borrowed.");
            if (item.IsOnLoan) throw new InvalidOperationException("Item is already on loan.");
            if (ActiveLoanCount() >= MaxLoans) throw new InvalidOperationException("Member has reached maximum active loan limit.");

            var loan = new Loan(loanId, borrowDate, this, item);
            _loans.Add(loan);
            item.SetOnLoan(true);
            return loan;
        }

        public void ReturnLoan(Loan loan, DateOnly returnDate)
        {
            if (!_loans.Contains(loan)) throw new InvalidOperationException("Loan does not belong to this member.");

            loan.CompleteReturn(returnDate);
        }
    }
}
