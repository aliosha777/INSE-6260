namespace Banking.BankingOperationsEngine
{
    using Banking.Models;

    public class TransactionEngine
    {
        public void DebitTransaction(IAccount source, IAccount destination, decimal value)
        {
        }

        public void CreditTransaction(IAccount source, IAccount destination, decimal value)
        {
        }

        private bool HasSufficientFunds(IAccount account, decimal value)
        {
            // Account balance plus pending transactions
            return true;
        }
    }
}