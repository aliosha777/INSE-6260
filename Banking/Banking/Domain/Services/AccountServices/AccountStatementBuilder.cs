using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Domain.Services.AccountServices
{
    using Banking.Application.DAL;
    using Banking.Application.Web.ViewModels;
    using Banking.Domain.Entities;
    using Banking.Domain.Services.BankingOperationsEngine;

    public class AccountStatementBuilder
    {
        public AccountStatement BuildAccountStatement(
            IAccount account, 
            DateTime from, 
            DateTime to,
            double availableBalance,
            IEnumerable<ITransaction> accountTransactions)
        {
            var statement = new AccountStatement
            {
                AccountNumber = account.AccountNumber,
                AccountType = account.Type,
                AvailableBalance = availableBalance,
                From = from,
                To = to
            };

            var statementLines = new List<AccountStatementLine>();

            var prevBalance = account.Balance;
            var prevTransValue = 0.0;

            // Could order by date applied as well
            foreach (var transaction in accountTransactions.OrderByDescending(t => t.TransactionId))
            {
                var statementLine = new AccountStatementLine
                    {
                        Applied = transaction.Applied,
                        Description = transaction.Description,
                        TransactionId = transaction.TransactionId
                    };

                // The accounts in question are liability accounts 
                // so left means withdrawal and right means deposit
                if (account.AccountId == transaction.LeftAccount.AccountId)
                {
                    statementLine.Withdrawal = transaction.Value.ToString();
                    statementLine.AccountBalance = (double)prevBalance + prevTransValue;
                    prevTransValue = (double)transaction.Value;
                }
                else
                {
                    statementLine.Deposit = transaction.Value.ToString();
                    statementLine.AccountBalance = (double)prevBalance + prevTransValue;
                    prevTransValue = -(double)transaction.Value;
                }

                prevBalance = (decimal)statementLine.AccountBalance;

                statementLines.Add(statementLine);
            }

            statementLines.Reverse();
            statement.StatementLines.AddRange(statementLines);

            return statement;
        }
    }
}