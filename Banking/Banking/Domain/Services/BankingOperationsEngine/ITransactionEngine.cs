using System.Collections.Generic;

using Banking.Domain.Entities;

namespace Banking.Domain.Services.BankingOperationsEngine
{
    public interface ITransactionEngine
    {
        void ApplyTransaction(Transaction transaction);

        ITransaction CreateTransaction(
            IAccount leftAccount, 
            IAccount rightAccount, 
            decimal amount);
    }
}