using System.Collections.Generic;

using Banking.Domain.Entities;
using Banking.Models;

namespace Banking.BankingOperationsEngine
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