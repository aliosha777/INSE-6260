using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Domain.Services.AccountServices
{
    public class AccountStatementLine
    {
        public int TransactionId { get; set; }

        public DateTime? Applied { get; set; }

        public string Description { get; set; }

        public string Withdrawal { get; set; }

        public string Deposit { get; set; }

        public double AccountBalance { get; set; }
    }
}