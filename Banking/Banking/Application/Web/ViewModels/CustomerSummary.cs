using System.Collections.Generic;

using Banking.Domain.Entities;

namespace Banking.ViewModels
{
    public class CustomerSummary
    {
        public CustomerSummary()
        {
            Accounts = new List<IAccount>();
            CurrentAddress = new AddressViewModel();
        }

        public ICollection<IAccount> Accounts { get; set; }

        public AddressViewModel CurrentAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}