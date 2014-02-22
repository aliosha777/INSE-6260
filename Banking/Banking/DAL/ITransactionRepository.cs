namespace Banking.DAL
{
    using System.Collections.Generic;

    using Banking.Models;

    public interface ITransactionRepository
    {
        Transaction GetTransactionById(int transactionId);

        IEnumerable<Transaction> GetAccountTransactions(IAccount account);
    }
}