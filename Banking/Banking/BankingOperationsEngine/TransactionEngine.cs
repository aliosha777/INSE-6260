using System;
using System.Collections.Generic;
using System.Linq;

using Banking.Domain.Entities;
using Banking.Exceptions;
using Banking.Models;

namespace Banking.BankingOperationsEngine
{
    public class TransactionEngine : ITransactionEngine
    {
        public Transaction CreateTransferTransaction(
            IAccount source, 
            IAccount destination, 
            decimal transactionValue,
            IEnumerable<ITransaction> pendingTransactions)
        {
            if (!source.IsActive)
            {
                throw new BankingValidationException(
                    "Cannot perform transactions on an account that is not active. Account number: " + source.AccountNumber);
            }

            if (!destination.IsActive)
            {
                throw new BankingValidationException(
                    "Cannot perform transactions on an account that is not active. Account number: " + destination.AccountNumber);
            }

            var transaction = new Transaction
            {
                LeftAccount = source,
                RightAccount = destination,
                Value = transactionValue,
                Type = TransactionTypes.Transfer,
                Created = new DateTime()
            };

            if (HasSufficientFunds(source, transactionValue, pendingTransactions))
            {
                transaction.Status = TransactionStatus.Pending;
            }
            else
            {
                transaction.Status = TransactionStatus.Failed;
            }

            return transaction;
        }

        public void ApplyTransaction(Transaction transaction)
        {
        }

        private void DebitAccount(IAccount account, decimal amount)
        {
        }

        private void CreditAccount(IAccount account, decimal amount)
        {
        }

        /// <summary>
        /// Expects a list of pending transactions that debit or credit this account
        /// </summary>
        /// <param name="account"></param>
        /// <param name="value"></param>
        /// <param name="transactions"></param>
        /// <returns></returns>
        private bool HasSufficientFunds(IAccount account, decimal value, IEnumerable<ITransaction> transactions)
        {
            // Account balance plus pending transactions
            decimal totalPending =
                transactions
                .Where(transaction => transaction.Status == TransactionStatus.Pending)
                .Sum(transaction => transaction.Value);

            return totalPending <= account.Balance + value;
        }

        private bool SetLock(IAccount account)
        {
            return true;
        }
    }
}