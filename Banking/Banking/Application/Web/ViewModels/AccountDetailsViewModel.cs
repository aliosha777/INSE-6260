using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Application.Web.ViewModels
{
    public class AccountDetailsViewModel
    {
        public AccountDetailsViewModel()
        {
            Transactions = new List<TransactionViewModel>();
        }

        public AccountViewModel Account { get; set; }

        public List<TransactionViewModel> Transactions { get; set; } 
    }
}