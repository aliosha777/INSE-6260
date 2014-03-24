using System.Collections.Generic;

namespace Banking.Application.Web.ViewModels
{
    public class CustomerSummary
    {
        public CustomerSummary()
        {
            Accounts = new List<AccountViewModel>();
            CurrentAddress = new AddressViewModel();
        }

        public List<AccountViewModel> Accounts { get; set; }

        public AddressViewModel CurrentAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}