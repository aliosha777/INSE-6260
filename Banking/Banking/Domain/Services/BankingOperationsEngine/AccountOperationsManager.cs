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

        private readonly ITimeProvider timeProvider;

        public AccountOperationsManager(
            ITransactionEngine transactionEngine,
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            ICustomerRepository customerRepository,
            ITimeProvider timeProvider)
        {
            this.transactionEngine = transactionEngine;
            this.transactionRepository = transactionRepository;
            this.accountRepository = accountRepository;
            this.customerRepository = customerRepository;
            this.timeProvider = timeProvider;
        }

        public IAccount CreateAccount(AccountTypes accountType, ICustomer owner)
        {
            if (accountType == AccountTypes.GeneralLedgerCash)
            {
                throw new BankingValidationException("Cannot create General Cash account!!");
            }

            var creationDate = timeProvider.Now();

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

        public bool Withdraw(ICustomer customer, int accountId, double amount)
        {
            var cashAccount = accountRepository.GetGeneralLedgerCashAccount();
            var account = this.GetCustomerAccount(customer, accountId);
            var pendingTransactions = transactionRepository.GetAccountTransactions(account);

            var hasSufficientFunds = HasSufficientFunds(account, (decimal)amount, pendingTransactions);

            if (hasSufficientFunds)
            {
                var transaction = 
                    transactionEngine.CreateTransaction(account, cashAccount, (decimal)amount);
                transactionRepository.AddTransaction(transaction);
                transactionRepository.SaveChanges();
            }

            return hasSufficientFunds;
        }

        public bool Transfer(ICustomer customer, int sourceAccountId, int destinationAccountId, double amount)
        {
            if (sourceAccountId == destinationAccountId)
            {
                throw new BankingValidationException("Source and target accounts cannot be the same");
            }

            var source = this.GetCustomerAccount(customer, sourceAccountId);
            var destination = this.GetCustomerAccount(customer, destinationAccountId);

            var pendingTransactions = transactionRepository.GetAccountTransactions(source);
            var hasSufficientFunds = HasSufficientFunds(source, (decimal)amount, pendingTransactions);

            if (hasSufficientFunds)
            {
                var transaction = transactionEngine.CreateTransaction(source, destination, (decimal)amount);

                transactionRepository.AddTransaction(transaction);
                transactionRepository.SaveChanges();
            }

            return hasSufficientFunds;
        }

        /// <summary>
        /// Calculates the balance of an account at a time in the past
        /// </summary>
        /// <param name="account"> </param>
        /// <param name="balancePoint"> </param>
        /// <returns></returns>
        public double Backtrack(IAccount account, DateTime balancePoint)
        {
            if (account.Category != AccountCategories.Liability)
            {
                throw new BankingValidationException("Balance backtracking only works on liability accounts");
            }

            var amountAtBalancePoint = account.Balance;
            var today = timeProvider.Now().Date;
            var transactions = transactionRepository.GetTransactionRange(account, balancePoint, today);

            var orderedTransactions =
                transactions
                .Where(t => t.Status == TransactionStatus.Applied)
                .OrderByDescending(t => t.Applied);

            foreach (var transaction in orderedTransactions)
            {
                // account on the left => credit
                // liability + credit => increase (deposit)
                // reverse => decrease balance
                if (account.AccountId == transaction.LeftAccount.AccountId)
                {
                    amountAtBalancePoint -= transaction.Value;
                }
                else
                {
                    if (account.AccountId == transaction.RightAccount.AccountId)
                    {
                        amountAtBalancePoint += transaction.Value;
                    }
                }
            }

            return (double)amountAtBalancePoint;
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