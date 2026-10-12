using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem
{
    public class StudentMember : Member
    {
        public StudentMember(string personId, string fullName, string phone)
            : base(personId, fullName, phone, maxLoans: 3, feeDiscountPercentage: 0m)
        {
        }
    }
}
