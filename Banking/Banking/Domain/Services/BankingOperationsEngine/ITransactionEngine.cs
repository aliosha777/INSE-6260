using System.Collections.Generic;

using Banking.Domain.Entities;

namespace Banking.Domain.Services.BankingOperationsEngine
{
    using System;

    public interface ITransactionEngine
    {
        void ApplyTransaction(ITransaction transaction);

        ITransaction CreateTransaction(
            IAccount leftAccount, 
            IAccount rightAccount, 
            decimal amount,
            string description);

        bool IsTransactionDue(ITransaction transaction);
    }
}