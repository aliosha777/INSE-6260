using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.DAL
{
    using System.Data;

    using Banking.Models;

    public class AccountRepository : IAccountRepository
    {
        private BankDBContext context;
        private bool disposed = false;

        public AccountRepository(BankDBContext context)
        {
            this.context = context;
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Account GetAccountById(int accountId)
        {
            return context.Accounts.Find(accountId);
        }

        public void InsertAccount(Account account)
        {
            context.Accounts.Add(account);
        }

        public void UpdateAccount(Account account)
        {
            context.Entry(account).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}