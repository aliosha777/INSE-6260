using Banking.Domain.Entities;

namespace Banking.Domain.Services.BankingOperationsEngine
{
    public interface IAccountOperationsManager
    {
        void Deposit(ICustomer customer, int accountId, double amount);

        bool Withdraw(ICustomer customer, int accountId, double amount);

        bool Transfer(ICustomer customer, int sourceAccountId, int destinationAccountId, double amount);

        IAccount CreateAccount(AccountTypes accountType, ICustomer owner);
    }
}