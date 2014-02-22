using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Banking.Models;

namespace Banking.DAL
{
    public class TransactionRepository : ITransactionRepository
    {
        private BankDBContext context;

        public TransactionRepository(BankDBContext context)
        {
            this.context = context;
        }

        public Transaction GetTransactionById(int transactionId)
        {
            return null;
        }

        public IEnumerable<Transaction> GetAccountTransactions(IAccount account)
        {
            return null;
        }
    }
}