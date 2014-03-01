using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Domain.Entities
{
    using Banking.Models;

    public class Transaction : ITransaction
    {
        public Transaction()
        {
        }

        public int TransactionId { get; set; }

        public IAccount LeftAccount { get; set; }

        public IAccount RightAccount { get; set; }

        public decimal Value { get; set; }

        public DateTime Created { get; set; }

        public TransactionStatus Status { get; set; }
    }
}