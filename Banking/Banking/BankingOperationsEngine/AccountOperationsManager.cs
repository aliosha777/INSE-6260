using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.BankingOperationsEngine
{
    using Banking.DAL;
    using Banking.Models;

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
            // Credit the general ledger and debit the account?
        }

        public void Withdraw(IAccount account, decimal amount)
        {
            // Debit the general ledger and credit the account?
        }

        public void Transfer(IAccount source, IAccount destination, decimal amount)
        {
            var transaction = transactionEngine.CreateTransferTransaction();
        }
    }
}