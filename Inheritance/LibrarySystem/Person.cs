using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem
{
    public class Person
    {
        public string PersonId { get; }
        public string FullName { get; }
        public string Phone { get; }

        protected Person(string personId, string fullName, string phone)
        {
            if (string.IsNullOrWhiteSpace(personId)) throw new ArgumentException("Person ID cannot be empty.");
            if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Full Name cannot be empty.");
            if (string.IsNullOrWhiteSpace(phone)) throw new ArgumentException("Phone cannot be empty.");

            PersonId = personId;
            FullName = fullName;
            Phone = phone;
        }
    }
}
