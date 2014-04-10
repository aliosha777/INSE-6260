using Banking.Domain.Entities;

namespace Banking.Domain.Services.BankingOperationsEngine
{
    public interface IAccountOperationsManager
    {
        void Deposit(ICustomer customer, int accountId, double amount);

        void Withdraw(ICustomer customer, int accountId, double amount);

        void Transfer(IAccount source, IAccount destination, decimal amount);

        IAccount CreateAccount(AccountTypes accountType, ICustomer owner);
    }
}