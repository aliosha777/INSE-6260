using System.Collections.Generic;

using Banking.Domain.Entities;
using Banking.Models;

namespace Banking.Core
{
    public static class DomainMapperExtensionMethods
    {
        public static BankAccountModel ToModel(this IAccount account)
        {
            var bankAccountModel = new BankAccountModel
            {
                AccountId = account.AccountId,
                AccountNumber = account.AccountNumber,
                Balance = account.Balance,
                Created = account.Created,
                IsActive = account.IsActive,
                IsLocked = account.IsLocked,
                Modified = account.Modified,
                Type = account.Type,
                Category = account.Category,
                Owners = new List<CustomerModel>()
            };

            foreach (var customer in account.Owners)
            {
                bankAccountModel.Owners.Add(customer.ToModel());
            }

            return bankAccountModel;
        }

        public static IAccount ToAccount(this BankAccountModel bankAccountModel)
        {
            var account = new Account
            {
                AccountId = bankAccountModel.AccountId,
                AccountNumber = bankAccountModel.AccountNumber,
                Balance = bankAccountModel.Balance,
                Created = bankAccountModel.Created,
                IsActive = bankAccountModel.IsActive,
                IsLocked = bankAccountModel.IsLocked,
                Modified = bankAccountModel.Modified,
                Owners = new List<ICustomer>(),
                Type = bankAccountModel.Type,
                Category = bankAccountModel.Category
            };

            foreach (var customerModel in bankAccountModel.Owners)
            {
                account.Owners.Add(customerModel.ToCustomer());
            }

            return account;
        }

        public static ITransaction ToTransaction(this TransactionModel transactionModel)
        {
            var transaction = new Transaction
            {
                TransactionId = transactionModel.TransactionId,
                LeftAccount = transactionModel.LeftAccount.ToAccount(),
                RightAccount = transactionModel.RightAccount.ToAccount(),
                Value = transactionModel.Value,
                Created = transactionModel.Created,
                Status = transactionModel.Status
            };

            return transaction;
        }

        public static TransactionModel ToModel(this ITransaction transaction)
        {
            var transactionModel = new TransactionModel()
            {
                TransactionId = transaction.TransactionId,
                LeftAccount = transaction.LeftAccount.ToModel(),
                RightAccount = transaction.RightAccount.ToModel(),
                Value = transaction.Value,
                Created = transaction.Created,
                Status = transaction.Status
            };

            return transactionModel;
        }

        public static CustomerModel ToModel(this ICustomer customer)
        {
            var customerModel = new CustomerModel
                {
                    Accounts = new List<BankAccountModel>(),
                    Addresses = new List<AddressModel>(),
                    CustomerId = customer.CustomerId,
                    Email = customer.Email,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Phone = customer.Phone
                };

            foreach (var account in customer.Accounts)
            {
                customerModel.Accounts.Add(account.ToModel());
            }

            foreach (var address in customer.Addresses)
            {
                customerModel.Addresses.Add(address.ToModel());
            }

            return customerModel;
        }

        public static ICustomer ToCustomer(this CustomerModel customerModel)
        {
            var customer = new Customer
                {
                    Accounts = new List<IAccount>(),
                    Addresses = new List<IAddress>(),
                    CustomerId = customerModel.CustomerId,
                    Email = customerModel.Email,
                    FirstName = customerModel.FirstName,
                    LastName = customerModel.LastName,
                    Phone = customerModel.Phone
                };

            foreach (var accountModel in customerModel.Accounts)
            {
                customer.Accounts.Add(accountModel.ToAccount());
            }

            foreach (var addressModel in customerModel.Addresses)
            {
                customer.Addresses.Add(addressModel.ToAddress());
            }

            return customer;
        }

        public static IAddress ToAddress(this AddressModel addressModel)
        {
            var address = new Address
            {
                AddressId = addressModel.AddressId,
                City = addressModel.City,
                IsActive = addressModel.IsActive,
                Line1 = addressModel.Line1,
                Line2 = addressModel.Line2,
                PostalCode = addressModel.PostalCode,
                Province = addressModel.Province
            };

            return address;
        }

        public static AddressModel ToModel(this IAddress address)
        {
            var addressModel = new AddressModel
            {
                AddressId = address.AddressId,
                City = address.City,
                IsActive = address.IsActive,
                Line1 = address.Line1,
                Line2 = address.Line2,
                PostalCode = address.PostalCode,
                Province = address.Province
            };

            return addressModel;
        }

        public static InvestmentIntervalModel ToModel(this IInvestmentInterval investmentInterval)
        {
            var investmentIntervalModel = new InvestmentIntervalModel
                {
                    
                };

            return investmentIntervalModel;
        }

        public static InvestmentInterval ToInvestmentInterval(this InvestmentIntervalModel investmentIntervalModel)
        {
            var investmentInterval = new InvestmentInterval { };

            return investmentInterval;
        }

        public static Investment ToInvestment(this InvestmentModel investmentModel)
        {
            var investment = new Investment
            {
                TermStart = investmentModel.TermStart,
                TermEnd = investmentModel.TermEnd,
                CompoundingFrequency = investmentModel.CompoundingFrequency,
                InvestmentIntervals = new List<IInvestmentInterval>(),
                Type = investmentModel.Type
            };

            foreach (var interval in investmentModel.InvestmentIntervals)
            {
                investment.InvestmentIntervals.Add(interval.ToInvestmentInterval());
            }

            return investment;
        }

        public static InvestmentModel ToModel(this Investment investment)
        {
            var investmentModel = new InvestmentModel
            {
                Account = null,
                CompoundingFrequency = investment.CompoundingFrequency,
                InvestmentIntervals = new List<InvestmentIntervalModel>(),
                TermEnd = investment.TermEnd,
                TermStart = investment.TermStart,
                Type = investment.Type
            };

            foreach (var interval in investment.InvestmentIntervals)
            {
                investmentModel.InvestmentIntervals.Add(interval.ToModel());
            }

            return investmentModel;
        }
    }
}