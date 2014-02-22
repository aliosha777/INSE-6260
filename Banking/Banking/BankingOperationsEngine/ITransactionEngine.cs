namespace Banking.BankingOperationsEngine
{
    using System.Collections.Generic;

    using Banking.Models;

    public interface ITransactionEngine
    {
        Transaction CreateTransferTransaction(
            IAccount source, 
            IAccount destination, 
            decimal transactionValue,
            IEnumerable<Transaction> pendingTransactions);

        void ApplyTransaction(Transaction transaction);
    }
}