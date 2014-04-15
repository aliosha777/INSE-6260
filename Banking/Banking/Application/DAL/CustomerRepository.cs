using System.Linq;
using System;

using Banking.Domain.Core;
using Banking.Domain.Entities;

namespace Banking.Application.DAL
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    using Banking.Application.Models;

    using WebMatrix.WebData;

    public class CustomerRepository : ICustomerRepository
    {
        private BankDBContext context;
        private bool disposed;

        public CustomerRepository(BankDBContext context)
        {
            this.context = context;
        }

        public ICustomer GetCustomerById(int customerId)
        {
            var customerModel = 
                context
                .Customers
                .Include("Accounts")
                .Include("Addresses")
                .FirstOrDefault(c => c.CustomerId == customerId);

            return customerModel == null ? null : customerModel.ToCustomer();
        }

        public void AddCustomer(ICustomer customer)
        {
            var customerModel = customer.ToModel();
            context.Customers.Add(customerModel);

            // Hack until proper role management is implemented
            context.Database.ExecuteSqlCommand(
                @"
                    declare @roleId int
                    select @roleId = RoleId from webpages_Roles where RoleName like 'Customer'
                    insert into webpages_UsersInRoles (UserId, RoleId) values (@userId,@roleId)",
                new SqlParameter("userId", WebSecurity.GetUserId(customer.UserName)));

            context.SaveChanges();
            
            customer.CustomerId = customerModel.CustomerId;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        /// <summary>
        /// Updates an already existing customer.
        /// The assumption is that related entities with Id == 0 are not saved to the db yet.
        /// Accounts and addresses cannot be deleted; they are marked as inactive instead.
        /// An updated customer object is returned. The passes cutomer object is not to be used.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="saveImmediately"></param>
        public ICustomer UpdateCustomer(ICustomer customer, bool saveImmediately = false)
        {
            var customerModel = customer.ToModel();

            CustomerModel trackedEntity = context.Customers.Find(customerModel.CustomerId);
            
            // Sync scalar values to the tracked object. 
            context.Entry(trackedEntity).CurrentValues.SetValues(customerModel);
            
            // Sync Addresses 
            foreach (var address in customerModel.Addresses)
            {
                if (address.AddressId == 0)
                {
                    trackedEntity.Addresses.Add(address);
                }
            }

            // Sync Accounts
            // A messy way to track new accounts and set their Ids back to the original object
            var updatedAccounts = new List<Tuple<IAccount, BankAccountModel>>();

            foreach (var account in customer.Accounts)
            {
                var accountModel = account.ToModel();
                
                if (account.AccountId == 0)
                {
                    updatedAccounts.Add(new Tuple<IAccount, BankAccountModel>(account, accountModel));
                    trackedEntity.Accounts.Add(accountModel);
                }
                else
                {
                    // update the account from the model
                    var trackedAccount = trackedEntity.Accounts.Single(a => a.AccountId == accountModel.AccountId);
                    context.Entry(trackedAccount).CurrentValues.SetValues(accountModel);
                }
            }

            context.Entry(trackedEntity).State = EntityState.Modified;

            if (saveImmediately)
            {
                context.SaveChanges();
            }
            
            // At this point all AccountModel objects in newAccounts should have Ids
            foreach (var accountTuple in updatedAccounts)
            {
                var account = customer.Accounts.First(a => a == accountTuple.Item1);
                account.AccountId = accountTuple.Item2.AccountId;
            }

            ////var entry = context.Entry(customerModel);

            ////if (entry.State == EntityState.Detached)
            ////{
            ////    var set = context.Set<CustomerModel>();
            ////    var attachedEntity = set.Local.SingleOrDefault(e => e.CustomerId == customerModel.CustomerId);

            ////    if (attachedEntity != null)
            ////    {
            ////        var attachedEntry = context.Entry(attachedEntity);
            ////        attachedEntry.CurrentValues.SetValues(customerModel);
            ////    }
            ////    else
            ////    {
            ////        entry.State = EntityState.Modified;
            ////    }

            ////    if (saveImmediately)
            ////    {
            ////        context.SaveChanges();
            ////    }
            ////}

            return trackedEntity.ToCustomer();
        }

        public IEnumerable<ICustomer> GetCustomersByFirstName(string firstName)
        {
            var customers = context.Customers.Where(c => c.FirstName.Contains(firstName));

            foreach(var customer in customers)
            {
                yield return customer.ToCustomer();
            }
        }

        public IEnumerable<ICustomer> GetCustomersByUserName(string userName)
        {
            var customers = context.Customers.Where(c => c.UserName == userName);

            foreach (var customer in customers)
            {
                yield return customer.ToCustomer();
            }
        }

        public IEnumerable<ICustomer> GetCustomersByAccountNumber(string accountNumber)
        {
            var customers = context.Customers.Where(c => c.Accounts.Any(a => a.AccountNumber == accountNumber));

            foreach (var customer in customers)
            {
                yield return customer.ToCustomer();
            }
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

        // Hack for now
        private void AttachEntity<TEntity>(TEntity entity) where TEntity : AddressModel
        {
            var entry = context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                var set = context.Set<TEntity>();
                var attachedEntity = set.Local.SingleOrDefault(e => e.AddressId == entity.AddressId);

                if (attachedEntity != null)
                {
                    var attachedEntry = context.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entity);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }
    }
}