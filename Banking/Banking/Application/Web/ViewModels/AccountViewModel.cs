using System;
using Banking.Domain.Entities;

namespace Banking.Application.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class AccountViewModel
    {
        public string AccountNumber { get; set; }

        public int AccountId { get; set; }

        public AccountTypes AccountType { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Balance { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime Created { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime Modified { get; set; }
    }
}