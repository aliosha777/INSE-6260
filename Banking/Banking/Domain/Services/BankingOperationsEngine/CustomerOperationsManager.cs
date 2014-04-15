using System.Linq;
using Banking.Domain.Entities;
using Banking.Exceptions;

namespace Banking.Domain.Services.BankingOperationsEngine
{
    using System.Collections.Generic;

    using Banking.Application.DAL;

    public class CustomerOperationsManager : ICustomerOperationsManager
    {
        private readonly IInvestmentRepository investmentRepository;

        public CustomerOperationsManager(IInvestmentRepository investmentRepository)
        {
            this.investmentRepository = investmentRepository;
        }

        public ICustomer CreateCustomer(string userName)
        {
            var customer = new Customer
            {
                UserName = userName
            };

            return customer;
        }

        public IAddress GetActiveAddress(ICustomer customer)
        {
            // TODO: Verify this does not result in multiple database queries
            var activeAddresses = customer.Addresses.Where(a => a.IsActive);

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

        public IEnumerable<IInvestment> GetInvestments(ICustomer customer)
        {
            var investmentAccounts = customer.Accounts.Where(a => a.Type == AccountTypes.Investment);

            var investments = new List<IInvestment>();

            foreach (var account in investmentAccounts)
            {
                investments.AddRange(investmentRepository.GetAccountInvestments(account.AccountId));
            }

            return investments;
        }
    }
}