using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Application.Core
{
    using System.Web.Mvc;

    using Banking.Application.Web.ViewModels;
    using Banking.Domain.Entities;
    using Banking.Domain.Services.AccountServices;

    public static class ViewHelpers
    {
        public static IEnumerable<SelectListItem> GetAccountsSelectList(IEnumerable<IAccount>  accounts)
        {
            var values =
                accounts
                .Select(
                    account => new SelectListItem()
                    {
                        Value = account.AccountId.ToString(),
                        Text = FormatAccountDropdownItem(account)
                    });

            return values;
        }

        public static string FormatAccountDropdownItem(IAccount account)
        {
            return string.Format(
                "{0} {1} {2}",
                account.Type.ToString().PadRight(12, '\xA0'),
                account.AccountNumber,
                account.Balance.ToString("C").PadLeft(10, '\xA0'));
        }

        public static IEnumerable<SelectListItem> GetAccountTypesSelectList(IEnumerable<AccountTypes> excludedTypes)
        {
            var values = from int e in Enum.GetValues(typeof(AccountTypes))
                         where !excludedTypes.Contains((AccountTypes)e)
                         select
                             new SelectListItem
                                 {
                                     Value = e.ToString(), 
                                     Text = Enum.GetName(typeof(AccountTypes), e)
                                 };
            return values;
        }

        public static AccountDetailsViewModel CreateAccountDetailsViewModel(IAccount account, IEnumerable<ITransaction> accountTransactions)
        {
            var accountDetails = new AccountDetailsViewModel
            {
                Account = account.ToViewModel()
            };

            foreach (var transaction in accountTransactions)
            {
                accountDetails.Transactions.Add(transaction.ToViewModel());
            }

            return accountDetails;
        }

        public static AccountStatementViewModel CreateAccountStatementViewModel(AccountStatement statement)
        {
            var statementViewModel = new AccountStatementViewModel
            {
                AccountNumber = statement.AccountNumber,
                AccountType = Enum.GetName(typeof(AccountTypes), statement.AccountType),
                Balance = statement.AvailableBalance,
                From = statement.From,
                To = statement.To
            };

            foreach (var line in statement.StatementLines)
            {
                statementViewModel.Transactions.Add(new TransactionViewModel()
                {
                    AccountBalance = line.AccountBalance,
                    Applied = line.Applied,
                    Deposit = line.Deposit,
                    Withdrawal = line.Withdrawal,
                    Description = line.Description,
                    TransactionId = line.TransactionId
                });
            }

            return statementViewModel;
        }

        public static IEnumerable<SelectListItem> GetEnumSelectList(Type enumType)
        {
            var values = from int e in Enum.GetValues(enumType)
                         select new SelectListItem()
                         {
                             Value = e.ToString(),
                             Text = Enum.GetName(enumType, e)
                         };

            return values.ToList();
        }
    }
}