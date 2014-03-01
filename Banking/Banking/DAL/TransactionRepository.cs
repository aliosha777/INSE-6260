using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Banking.Core;

using Banking.Domain.Entities;

namespace Banking.DAL
{
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
            var transactions =
                context
                .Transactions
                .Where(
                    t => 
                        t.LeftAccount.AccountId == account.AccountId 
                        || t.RightAccount.AccountId == account.AccountId);

            return transactions.Select(t => t.ToTransaction());
        }

        public void AddTransaction(ITransaction transaction)
        {
            var transactionModel = transaction.ToModel();
            context.Transactions.Add(transactionModel);
            context.Entry(transactionModel).State = EntityState.Added;
        }

        // TODO: change this to Update only and have a separate add method
        public void UpdateTransaction(ITransaction transaction)
        {
            // Might need to lock this operation or use optimistic concurency management
            var transactionModel = transaction.ToModel();

            if (context.Transactions.Find(transaction.TransactionId) != null)
            {
                context.Transactions.Add(transactionModel);
                context.Entry(transactionModel).State = EntityState.Added;
            }
            else
            {
                context.Entry(transactionModel).State = EntityState.Modified; 
            }
        }

        public void SaveChanges()
        {
            context.SaveChanges();
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