using System.Linq;
using System;

using Banking.Domain.Core;
using Banking.Domain.Entities;

namespace Banking.Application.DAL
{
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

            return customerModel.ToCustomer();
        }

        public void AddCustomer(ICustomer customer)
        {
            var customerModel = customer.ToModel();
            context.Customers.Add(customerModel);
        }

        public void Save()
        {
            context.SaveChanges();
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
    }
}