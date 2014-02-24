using System;
using System.Collections.Generic;

using Banking.Domain.Entities;

namespace Banking.DAL
{
    using Banking.Core;
    using Banking.Models;

    public class TransactionRepository : ITransactionRepository
    {
        private BankDBContext context;
        private bool disposed;

        public TransactionRepository(BankDBContext context)
        {
            this.context = context;
        }

        public ITransaction GetTransactionById(int transactionId)
        {
            var transactionModel = context.Transactions.Find(transactionId);

            return transactionModel.ToTransaction();
        }

        public IEnumerable<ITransaction> GetAccountTransactions(IAccount account)
        {
            return null;
        }

        public void SaveTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }

            this.disposed = true;
        }
    }
}