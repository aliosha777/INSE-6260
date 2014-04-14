using Banking.Domain.Entities;

namespace Banking.Domain.Services.BankingOperationsEngine
{
    using System.Collections.Generic;

    public interface ICustomerOperationsManager
    {
        IAddress GetActiveAddress(ICustomer customer);

        Customer CreateCustomer(string firstName, string lastName, string phone, string email);

        IAccount GetAccount(ICustomer customer, int accountId);

        IEnumerable<IInvestment> GetInvestments(ICustomer customer);
    }
}
