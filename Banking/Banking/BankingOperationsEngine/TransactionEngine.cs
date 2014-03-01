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
        public ITransaction CreateTransaction(
            IAccount leftAccount, 
            IAccount rightAccount, 
            decimal amount)
        {
            if (!leftAccount.IsActive)
            {
                throw new BankingValidationException(
                    "Cannot perform transactions on an account that is not active. Account number: " + leftAccount.AccountNumber);
            }

            if (!rightAccount.IsActive)
            {
                throw new BankingValidationException(
                    "Cannot perform transactions on an account that is not active. Account number: " + rightAccount.AccountNumber);
            }

            var transaction = new Transaction()
            {
                LeftAccount = leftAccount,
                RightAccount = rightAccount,
                Created = new DateTime(),
                Status = TransactionStatus.Pending,
                Value = amount
            };

            return transaction;
        }

        public void ApplyTransaction(Transaction transaction)
        {
            // Here where the actual accounts are debited/credited
            // Account balances will be updated and the transaction will be marked as applied
        }
    }
}