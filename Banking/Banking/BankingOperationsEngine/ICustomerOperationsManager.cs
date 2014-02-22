using Banking.Models;

namespace Banking.BankingOperationsEngine
{
    public interface ICustomerOperationsManager
    {
        Address GetActiveAddress(Customer customer);
    }
}
