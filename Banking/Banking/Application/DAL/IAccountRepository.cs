using System;
using System.Collections.Generic;
using Banking.Domain.Entities;

namespace Banking.DAL
{
    public interface IAccountRepository : IDisposable
    {
        IAccount GetAccountById(int accountId);

        void AddAccount(IAccount account);

        void UpdateAccount(IAccount account);

        void Save();

        IEnumerable<IAccount> GetAllAccounts(ICustomer customer);

        IAccount GetGeneralLedgerCashAccount();
    }
}
