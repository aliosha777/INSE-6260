using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Banking.Domain.Entities;

namespace Banking.Application.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class AccountStatementViewModel
    {
        public AccountStatementViewModel()
        {
            Transactions = new List<TransactionViewModel>();
        }

        public int AccountId { get; set; }

        public string AccountNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public double Balance { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime From { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime To { get; set; }

        public string AccountType { get; set; }

        public List<TransactionViewModel> Transactions { get; set; }
    }
}