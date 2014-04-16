using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Domain.Services.AccountServices
{
    using Banking.Domain.Entities;

    public class AccountStatement
    {
        public AccountStatement()
        {
            StatementLines = new List<AccountStatementLine>();
        }

        public string AccountNumber { get; set; }

        public AccountTypes AccountType { get; set; }

        public double AvailableBalance { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public List<AccountStatementLine> StatementLines { get; set; }
    }
}