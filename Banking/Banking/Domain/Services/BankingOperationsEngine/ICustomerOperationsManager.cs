using Banking.Domain.Entities;

namespace Banking.Domain.Services.BankingOperationsEngine
{
    public interface ICustomerOperationsManager
    {
        IAddress GetActiveAddress(ICustomer customer);

        Customer CreateCustomer(string firstName, string lastName, string phone, string email);
    }
}
