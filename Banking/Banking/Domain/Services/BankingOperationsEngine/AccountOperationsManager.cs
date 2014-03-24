using System.Collections.Generic;
using System.Linq;

using Banking.Application.DAL;
using Banking.Domain.Entities;
using Banking.Exceptions;
using Banking.Models;

namespace Banking.Domain.Services.BankingOperationsEngine
{
    public class AccountOperationsManager : IAccountOperationsManager
    {
        private ITransactionEngine transactionEngine;
        private IAccountRepository accountRepository;
        private ITransactionRepository transactionRepository;

        public AccountOperationsManager(
            ITransactionEngine transactionEngine,
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository)
        {
            this.transactionEngine = transactionEngine;
            this.transactionRepository = transactionRepository;
            this.accountRepository = accountRepository;
        }

        public void Deposit(IAccount account, decimal amount)
        {
            var cashAccount = accountRepository.GetGeneralLedgerCashAccount();
            var transaction = transactionEngine.CreateTransaction(cashAccount, account, amount);

            transactionRepository.AddTransaction(transaction);
            transactionRepository.SaveChanges();
        }

        public void Withdraw(IAccount account, decimal amount)
        {
            var cashAccount = accountRepository.GetGeneralLedgerCashAccount();
            var pendingTransactions = transactionRepository.GetAccountTransactions(account);

            if (HasSufficientFunds(account, amount, pendingTransactions))
            {
                var transaction = transactionEngine.CreateTransaction(account, cashAccount, amount);
                transactionRepository.AddTransaction(transaction);
                transactionRepository.SaveChanges();
            }
        }

        public void Transfer(IAccount source, IAccount destination, decimal amount)
        {
            var pendingTransactions = transactionRepository.GetAccountTransactions(source);

            if (HasSufficientFunds(source, amount, pendingTransactions))
            {
                var transaction = transactionEngine.CreateTransaction(source, destination, amount);

                transactionRepository.AddTransaction(transaction);
                transactionRepository.SaveChanges();
            }
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
            // For now only support verifying funds on liability accounts
            if (account.Category != AccountCategories.Liability)
            {
                throw new BankingValidationException("Cannot apply this function to an asset account");
            }

            decimal totalPending = 0;
            decimal totalDecrease = 0;
            decimal totalIncrease = 0;

            // we need to find out which transactions increase the balance and which ones decrease it
            // filter out transactions that are not pending if any
            foreach (var transaction in transactions.Where(transaction => transaction.Status == TransactionStatus.Pending))
            {
                if (account.AccountId == transaction.LeftAccount.AccountId)
                {
                    // Liability + left = decrease
                    totalDecrease += transaction.Value;
                }
                else
                {
                    if (account.AccountId == transaction.RightAccount.AccountId)
                    {
                        // Liability + right = increase;
                        totalDecrease += totalIncrease;
                    }
                }
            }

            totalPending = totalIncrease - totalDecrease;

            return account.Balance >= totalPending + value;
        }
    }
}