using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.BankingOperationsEngine
{
    using Banking.Exceptions;
    using Banking.Models;

    public class CustomerOperationsManager : ICustomerOperationsManager
    {
        public Address GetActiveAddress(Customer customer)
        {
            // TODO: Verify this does not result in multiple database queries
            var activeAddresses = customer.Addresses.Where(a => a.IsActive);

            if (!activeAddresses.Any())
            {
                throw new BankingValidationException("Customer must have an active address");
            }

            if (activeAddresses.Count() > 1)
            {
                throw new BankingValidationException("Customer cannot have more than one active address");
            }

            return activeAddresses.FirstOrDefault();
        }
    }
}