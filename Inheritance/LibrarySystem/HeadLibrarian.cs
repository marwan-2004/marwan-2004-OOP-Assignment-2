using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem
{
    public class HeadLibrarian : Staff
    {
        public HeadLibrarian(string personId, string fullName, string phone, DateOnly hireDate, decimal monthlySalary)
            : base(personId, fullName, phone, hireDate, monthlySalary)
        {
        }

        public override decimal GetMonthlyPay() => MonthlySalary + 400m;

        public void AdjustLateFee(LibraryItem item, decimal newBaseFee)
        {
            item.SetBaseLateFee(newBaseFee);
        }

        public void WithdrawItem(LibraryItem item) => item.Withdraw();
        public void RestoreItem(LibraryItem item) => item.Restore();
    }
}
