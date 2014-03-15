using System.Linq;

using Banking.Domain.Entities;

namespace Banking.DAL
{
    using System;

    using Banking.Core;

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
                .FirstOrDefault(c => c.CustomerId == customerId);

            return customerModel.ToCustomer();
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