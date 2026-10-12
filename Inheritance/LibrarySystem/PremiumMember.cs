using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem
{
    public class PremiumMember : Member
    {
        public int ReadingPoints { get; private set; }

        public PremiumMember(string personId, string fullName, string phone, decimal feeDiscountPercentage)
            : base(personId, fullName, phone, maxLoans: 10, feeDiscountPercentage: feeDiscountPercentage)
        {
        }

        public void AddPoints(int points)
        {
            if (points > 0)
            {
                ReadingPoints += points;
            }
        }
    }
}
