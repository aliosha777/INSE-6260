using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.DAL
{
    using Banking.Models;

    public class CustomerRepository : ICustomerRepository
    {
        private BankDBContext context;

        public CustomerRepository(BankDBContext context)
        {
            this.context = context;
        }

        public Customer GetCustomerById(int customerId)
        {
            var customer = context.Customers.Include("Accounts").FirstOrDefault(c => c.CustomerId == customerId);

            return customer;
        }
    }
}