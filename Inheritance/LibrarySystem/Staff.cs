using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem
{
    public class Staff : Person
    {
        public DateOnly HireDate { get; }
        public decimal MonthlySalary { get; private set; }

        protected Staff(string personId, string fullName, string phone, DateOnly hireDate, decimal monthlySalary)
            : base(personId, fullName, phone)
        {
            if (monthlySalary <= 0) throw new ArgumentException("Monthly salary must be positive.");

            HireDate = hireDate;
            MonthlySalary = monthlySalary;
        }

        public void GiveRaise(decimal percentage)
        {
            if (percentage <= 0) throw new ArgumentException("Raise percentage must be greater than zero.");

            MonthlySalary += MonthlySalary * (percentage / 100m);
        }

        public virtual decimal GetMonthlyPay() => MonthlySalary;
    }
}