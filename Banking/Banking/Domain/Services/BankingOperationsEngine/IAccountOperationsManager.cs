using Banking.Domain.Entities;

namespace Banking.Domain.Services.BankingOperationsEngine
{
    public interface IAccountOperationsManager
    {
        void Deposit(IAccount account, decimal amount);

        void Withdraw(IAccount account, decimal amount);

        void Transfer(IAccount source, IAccount destination, decimal amount);

        IAccount CreateAccount(AccountTypes accountType, ICustomer owner);
    }
}