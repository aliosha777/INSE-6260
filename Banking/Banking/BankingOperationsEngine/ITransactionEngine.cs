using System.Collections.Generic;

using Banking.Domain.Entities;
using Banking.Models;

namespace Banking.BankingOperationsEngine
{
    public interface ITransactionEngine
    {
        Transaction CreateTransferTransaction(
            IAccount source, 
            IAccount destination, 
            decimal transactionValue,
            IEnumerable<ITransaction> pendingTransactions);

        void ApplyTransaction(Transaction transaction);
    }
}