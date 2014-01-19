using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Models
{
    public class Transaction
    {
        public enum TransactionTypes
        {
            Credit,
            Debit
        }

        public TransactionTypes Type { get; set; }

        public Account Account1 { get; set; }

        public Account Account2 { get; set; }

        public decimal Value { get; set; }

        public DateTime Created { get; set; }
    }
}