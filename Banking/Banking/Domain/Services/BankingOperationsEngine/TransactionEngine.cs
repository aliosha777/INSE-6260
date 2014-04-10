using System;
using System.Collections.Generic;
using System.Linq;

using Banking.Application.DAL;
using Banking.Domain.Entities;
using Banking.Exceptions;
using Banking.Application.Models;

namespace Banking.Domain.Services.BankingOperationsEngine
{
    public class TransactionEngine : ITransactionEngine
    {
        private readonly ITransactionRepository transactionRepository;

        private readonly IAccountRepository accountRepository;

        public TransactionEngine(
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository)
        {
            this.transactionRepository = transactionRepository;
            this.accountRepository = accountRepository;
        }

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
                Created = DateTime.Now,
                Applied = null,
                Status = TransactionStatus.Pending,
                Value = amount
            };

            return transaction;
        }

        /// <summary>
        /// Here where the actual accounts are debited/credited
        /// Account balances will be updated and the transaction will be marked as applied
        /// </summary>
        /// <param name="transaction"></param>
        public void ApplyTransaction(ITransaction transaction)
        {
            if (transaction.Status != TransactionStatus.Pending)
            {
                throw new BankingValidationException("Cannot apply a transaction that is not pending");
            }

            if (!this.IsTransactionDue(transaction))
            {
                return;
            }

            var accountToBeDebited = transaction.LeftAccount;
            var accountToBeCredited = transaction.RightAccount;

            // Debiting an asset account increases its balance and creditig it decreases its balance

            decimal debitAccountDelta = 
                accountToBeDebited.Category == AccountCategories.Asset ? transaction.Value : -transaction.Value;

            decimal creditAccountDelta =
                accountToBeCredited.Category == AccountCategories.Asset ? -transaction.Value : transaction.Value;

            var modified = DateTime.Now;

            accountToBeDebited.Balance += debitAccountDelta;
            accountToBeDebited.Modified = modified;

            accountToBeCredited.Balance += creditAccountDelta;
            accountToBeCredited.Modified = modified;

            accountRepository.UpdateAccount(accountToBeDebited);
            accountRepository.UpdateAccount(accountToBeCredited);

            accountRepository.Save();

            transaction.Status = TransactionStatus.Applied;

            transactionRepository.UpdateTransaction(transaction);
            transactionRepository.SaveChanges();
        }

        public bool IsTransactionDue(ITransaction transaction)
        {
            var today = DateTime.Now;

            return 
                !transaction.Applied.HasValue 
                || transaction.Applied.GetValueOrDefault().Date <= today;
        }
    }
}