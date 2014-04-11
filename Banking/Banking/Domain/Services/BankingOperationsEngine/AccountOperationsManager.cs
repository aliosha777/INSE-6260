using System.Collections.Generic;
using System.Linq;

using Banking.Application.DAL;
using Banking.Domain.Entities;
using Banking.Exceptions;
using Banking.Application.Models;

namespace Banking.Domain.Services.BankingOperationsEngine
{
    using System;

    public class AccountOperationsManager : IAccountOperationsManager
    {
        private readonly ITransactionEngine transactionEngine;
        private readonly IAccountRepository accountRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly ICustomerRepository customerRepository;

        public AccountOperationsManager(
            ITransactionEngine transactionEngine,
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            ICustomerRepository customerRepository)
        {
            this.transactionEngine = transactionEngine;
            this.transactionRepository = transactionRepository;
            this.accountRepository = accountRepository;
            this.customerRepository = customerRepository;
        }

        public IAccount CreateAccount(AccountTypes accountType, ICustomer owner)
        {
            if (accountType == AccountTypes.GeneralLedgerCash)
            {
                throw new BankingValidationException("Cannot create General Cash account!!");
            }

            var creationDate = DateTime.Now;

            var account = new Account
                {
                    Balance = 0,
                    Type = accountType,
                    Category = AccountCategories.Liability,
                    Created = creationDate,
                    Modified = creationDate,
                    IsActive = true
                };

            account.Owners.Add(owner);
            owner.Accounts.Add(account);
            customerRepository.UpdateCustomer(owner, true);

            account.AccountNumber = GetNewAccountNumber(account.AccountId);

            // We have to save again to save the account number
            customerRepository.UpdateCustomer(owner, true);

            return account;
        }

        public void Deposit(ICustomer customer, int accountId, double amount)
        {
            var account = this.GetCustomerAccount(customer, accountId);

            var cashAccount = accountRepository.GetGeneralLedgerCashAccount();
            var transaction = transactionEngine.CreateTransaction(cashAccount, account, (decimal)amount);

            transactionRepository.AddTransaction(transaction);
            transactionRepository.SaveChanges();
        }

        public void Withdraw(ICustomer customer, int accountId, double amount)
        {
            var cashAccount = accountRepository.GetGeneralLedgerCashAccount();
            var account = this.GetCustomerAccount(customer, accountId);
            var pendingTransactions = transactionRepository.GetAccountTransactions(account);

            if (HasSufficientFunds(account, (decimal)amount, pendingTransactions))
            {
                var transaction = 
                    transactionEngine.CreateTransaction(account, cashAccount, (decimal)amount);
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
                        totalIncrease += transaction.Value;
                    }
                }
            }

            totalPending = totalDecrease - totalIncrease;

            return account.Balance >= totalPending + value;
        }

        private string GetNewAccountNumber(int accountId)
        {
            // TODO: Those numbers should be placed in a settings file
            const string BankId = "123";
            const string BranchId = "456";

            return string.Format("{0}-{1}-{2}", BankId, BranchId, accountId.ToString("D6"));
        }

        private IAccount GetCustomerAccount(ICustomer customer, int accountId)
        {
            var targetAccount =
                customer
                .Accounts
                .FirstOrDefault(account => account.AccountId == accountId);

            if (targetAccount == null)
            {
                throw new BankingValidationException(
                    string.Format("This customer does not have an Account with Id {0}", accountId));
            }

            return targetAccount;
        }
    }
}