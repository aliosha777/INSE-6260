using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Domain.Entities
{
    using Banking.Models;

    public class Customer : ICustomer
    {
        public Customer()
        {
            Addresses = new List<IAddress>();
            Accounts = new List<IAccount>();
        }

        public int CustomerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public List<IAddress> Addresses { get; set; }

        public List<IAccount> Accounts { get; set; }
    }
}