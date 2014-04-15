using Banking.Domain.Entities;

namespace Banking.Domain.Services.BankingOperationsEngine
{
    using System.Collections.Generic;

    public interface ICustomerOperationsManager
    {
        IAddress GetActiveAddress(ICustomer customer);

        ICustomer CreateCustomer(string userName);

        IAccount GetAccount(ICustomer customer, int accountId);

        IEnumerable<IInvestment> GetInvestments(ICustomer customer);
    }
}
