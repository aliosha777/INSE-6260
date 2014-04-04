using System.Linq;
using System;

using Banking.Domain.Core;
using Banking.Domain.Entities;

namespace Banking.Application.DAL
{
    using System.Data;

    using Banking.Application.Models;

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

            context.SaveChanges();

            customer.CustomerId = customerModel.CustomerId;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        /// <summary>
        /// Updates an already existing customer.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="saveImmediately"></param>
        public void UpdateCustomer(ICustomer customer, bool saveImmediately = false)
        {
            // TODO: update the customer object with the new address Id
            var customerModel = customer.ToModel();

            var trackedEntity = context.Customers.Find(customerModel.CustomerId);

            foreach (var address in customerModel.Addresses)
            {
                // Assume addresses that have been created but not yet saved to the db 
                // have no Id yet
                if (address.AddressId == 0)
                {
                    trackedEntity.Addresses.Add(address);
                }
            }

            foreach (var account in customerModel.Accounts)
            {
                if (account.AccountId == 0)
                {
                    trackedEntity.Accounts.Add(account);
                }
            }

            context.Entry(trackedEntity).CurrentValues.SetValues(customerModel);

            context.Entry(trackedEntity).State = EntityState.Modified;

            if (saveImmediately)
            {
                context.SaveChanges();
            }
            
            ////// Save new addresses first
            ////foreach (var address in customerModel.Addresses)
            ////{
            ////    if (address.AddressId == 0)
            ////    {
            ////        context.Addresses.Add(address);
            ////        context.Entry(address).State = EntityState.Added;
            ////    }
            ////    else
            ////    {
            ////        this.AttachEntity(address);
            ////    }
            ////}

            ////context.SaveChanges();

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