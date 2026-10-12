using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem
{
    public class Librarian : Staff
    {
        public Librarian(string personId, string fullName, string phone, DateOnly hireDate, decimal monthlySalary)
            : base(personId, fullName, phone, hireDate, monthlySalary)
        {
        }

        public void ProcessReturn(Member member, Loan loan, DateOnly returnDate)
        {
            member.ReturnLoan(loan, returnDate);
        }

        public void MarkAsLost(Loan loan)
        {
            loan.MarkAsLost();
        }
    }
}
