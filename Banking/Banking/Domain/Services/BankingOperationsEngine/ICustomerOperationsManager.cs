using Banking.Models;

namespace Banking.BankingOperationsEngine
{
    using Banking.Domain.Entities;

    public interface ICustomerOperationsManager
    {
        IAddress GetActiveAddress(ICustomer customer);
    }
}
