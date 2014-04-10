using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Banking.Application.Web.ViewModels;
using Banking.Domain.Entities;

namespace Banking.Application.Core
{
    public static class ViewModelMapperExtensionMethod
    {
        public static AccountViewModel ToViewModel(this IAccount account)
        {
           return new AccountViewModel
            {
                AccountNumber = account.AccountNumber,
                AccountId = account.AccountId,
                AccountType = account.Type, 
                Balance = account.Balance, 
                Created = account.Created, 
                Modified = account.Modified
            };
        }

        public static AddressViewModel ToViewModel(this IAddress address)
        {
            return new AddressViewModel
                {
                    Line1 = address.Line1,
                    Line2 = address.Line2,
                    City = address.City,
                    PostalCode = address.PostalCode,
                    Province = address.Province
                };
        }

        public static TransactionViewModel ToViewModel(this ITransaction transaction)
        {
            return new TransactionViewModel
            {
                TransactionId  = transaction.TransactionId,
                Value = transaction.Value,
                Created = transaction.Created,
                Applied = transaction.Applied,
                Status = transaction.Status
            };
        }
    }
}