using System;
using System.Collections.Generic;
using System.Data;

using Banking.Domain.Entities;

namespace Banking.DAL
{
    using System.Linq;

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

        public IEnumerable<IAccount> GetAllAccounts()
        {
            var accounts = context.Accounts;

            return accounts.Select(account => new Account(account)).ToList();
        }

        public IAccount GetAccountById(int accountId)
        {
            var account = context.Accounts.Find(accountId);

            return new Domain.Entities.Account(account);
        }

        public void InsertAccount(IAccount account)
        {
            var accountModel = new Banking.Models.Account();

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