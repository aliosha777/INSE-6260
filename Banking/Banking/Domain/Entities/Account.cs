using System;
using System.Collections.Generic;

namespace Banking.Domain.Entities
{
    public class Account : IAccount
    {
        public Account()
        {
            Owners = new List<ICustomer>();
        }

        public int AccountId { get; set; }

        public string AccountNumber { get; set; }

        public AccountTypes Type { get; set; }

        public AccountCategories Category { get; set; }

        public ICollection<ICustomer> Owners { get; set; }

        public decimal Balance { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public bool IsActive { get; set; }

        public bool IsLocked { get; set; }
    }
}