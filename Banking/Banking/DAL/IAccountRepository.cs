using System;
using System.Collections.Generic;
using Banking.Domain.Entities;

namespace Banking.DAL
{
    public interface IAccountRepository : IDisposable
    {
        IAccount GetAccountById(int accountId);

        void InsertAccount(IAccount account);

        void UpdateAccount(IAccount account);

        void Save();

        IEnumerable<IAccount> GetAllAccounts();
    }
}
