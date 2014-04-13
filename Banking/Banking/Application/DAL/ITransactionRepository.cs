using System;
using System.Collections.Generic;

using Banking.Domain.Entities;

namespace Banking.Application.DAL
{
    public interface ITransactionRepository : IDisposable
    {
        ITransaction GetTransactionById(int transactionId);

        IEnumerable<ITransaction> GetAccountTransactions(IAccount account);

        void UpdateTransaction(ITransaction transaction);

        void SaveChanges();

        void AddTransaction(ITransaction transaction);

        IEnumerable<ITransaction> GetPendingTransactions();

        IEnumerable<ITransaction> GetTransactionRange(IAccount account, DateTime from, DateTime to);
    }
}