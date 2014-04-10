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

        public IAccount GetAccount(ICustomer customer, int accountId)
        {
            var account = customer.Accounts.FirstOrDefault(a => a.AccountId == accountId);

            if (account == null)
            {
                throw new BankingValidationException("This customer does not have an account with Id " + accountId);
            }

            return account;
        }
    }
}