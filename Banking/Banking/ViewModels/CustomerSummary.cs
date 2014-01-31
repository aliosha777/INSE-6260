using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.ViewModels
{
    using Banking.Models;

    public class CustomerSummary
    {
        public CustomerSummary()
        {
            Accounts = new List<Account>();
            CurrentAddress = new Address();
        }

        public List<Account> Accounts { get; set; }

        public Address CurrentAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}