using System;
using System.Collections.Generic;

using Banking.Domain.Entities;

namespace Banking.DAL
{
    public interface ITransactionRepository : IDisposable
    {
        ITransaction GetTransactionById(int transactionId);

        IEnumerable<ITransaction> GetAccountTransactions(IAccount account);

        void AddOrUpdateTransaction(ITransaction transaction);

        void SaveChanges();

        void AddTransaction(ITransaction transaction);
    }
}