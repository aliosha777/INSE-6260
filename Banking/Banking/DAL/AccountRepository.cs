using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Banking.Domain.Entities;
using Banking.Models;

namespace Banking.DAL
{
    using Banking.Core;

    public class AccountRepository : IAccountRepository
    {
        private BankDBContext context;

        private bool disposed = false;

        public AccountRepository(BankDBContext context)
        {
            this.context = context;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IAccount GetGeneralLedgerCashAccount()
        {
            return null;
        }

        public IEnumerable<IAccount> GetAllAccounts(ICustomer customer)
        {
            var customerModel = customer.ToModel();

            var accounts = context.Accounts.Where(a => a.Owners.Contains(customerModel));

            return accounts.Select(accountModel => accountModel.ToAccount()).ToList();
        }

        public IAccount GetAccountById(int accountId)
        {
            var accountModel = context.Accounts.Find(accountId);

            return accountModel.ToAccount();
        }

        public void AddAccount(IAccount account)
        {
            var accountModel = new BankAccountModel();

            context.Accounts.Add(accountModel);
        }

        public void UpdateAccount(IAccount account)
        {
            context.Entry(account).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
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