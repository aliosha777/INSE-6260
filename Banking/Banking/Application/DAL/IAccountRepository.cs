using System;
using System.Collections.Generic;
using Banking.Domain.Entities;

namespace Banking.Application.DAL
{
    public interface IAccountRepository : IDisposable
    {
        IAccount GetAccountById(int accountId);

        void UpdateAccount(IAccount account, bool saveImmediately = false);

        void Save();

        IEnumerable<IAccount> GetAllAccounts(ICustomer customer);

        IAccount GetGeneralLedgerCashAccount();

        IAccount GetAccountByNumber(string accountNumber);
    }
}
