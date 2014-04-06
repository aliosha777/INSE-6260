using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Banking.Domain.Core;
using Banking.Domain.Entities;
using Banking.Application.Models;

namespace Banking.Application.DAL
{
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
            return 
                context
                .Accounts
                .Single(account => account.Type == AccountTypes.GeneralLedgerCash)
                .ToAccount();
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

            return accountModel == null ? null : accountModel.ToAccount();
        }

        public IAccount GetAccountByNumber(string accountNumber)
        {
            var accountModel = 
                context
                .Accounts
                .Include("Owners")
                .FirstOrDefault(a => a.AccountNumber == accountNumber);

            return accountModel == null ? null : accountModel.ToAccount();
        }

        public void AddAccount(IAccount account)
        {
            var accountModel = new BankAccountModel();

            context.Accounts.Add(accountModel);
        }

        public void UpdateAccount(IAccount account, bool saveImmediately = false)
        {
            var accountModel = account.ToModel();
            var trackedEntity = context.Accounts.Find(accountModel.AccountId);
            var entry = context.Entry(trackedEntity);

            entry.CurrentValues.SetValues(accountModel);
            entry.State = EntityState.Modified;
            
            if (saveImmediately)
            {
                context.SaveChanges();
            }
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