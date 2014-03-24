using System.Linq;
using Banking.Domain.Entities;
using Banking.Exceptions;

namespace Banking.Domain.Services.BankingOperationsEngine
{
    public class CustomerOperationsManager : ICustomerOperationsManager
    {
        public Customer CreateCustomer(string firstName, string lastName, string phone, string email)
        {
            var customer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                Email = email
            };

            return customer;
        }

        public IAddress GetActiveAddress(ICustomer customer)
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