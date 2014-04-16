using System;
using Banking.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;

namespace Banking.Tests.ApplicationLogic
{
    using System.Collections.Generic;

    using Banking.Application.DAL;
    using Banking.Application.Models;
    using Banking.Domain.Services.BankingOperationsEngine;

    using Ploeh.AutoFixture.AutoMoq;

    [TestClass]
    public class AccoutOperationsTest
    {
        [TestMethod]
        public void TestDeposit()
        {
            var fixture = new Fixture();

            var depositAmount = fixture.Freeze<decimal>();
            var created = fixture.Freeze<DateTime>();

            var customerAccount = fixture.Build<Account>().Without(a => a.Owners).Create();
            var cashAccount = fixture.Build<Account>().Without(a => a.Owners).Create();

            var customer = Mock.Of<ICustomer>();

            // Parameters to the class under test
            var accountRepository = Mock.Of<IAccountRepository>();
            var transactionRepository = Mock.Of<ITransactionRepository>();
            var transactionEngine = Mock.Of<ITransactionEngine>();
            var customerRepository = Mock.Of<ICustomerRepository>();
            var investmentRepository = Mock.Of<IInvestmentRepository>();
            var timeProvider = Mock.Of<ITimeProvider>();

            // This is the resulting transaction that we expect to be saved to the repository
            var transaction = 
                fixture
                .Build<Transaction>()
                .With(t => t.LeftAccount, cashAccount)
                .With(t => t.RightAccount, customerAccount)
                .With(t => t.Status, TransactionStatus.Pending)
                .With(t => t.Value, depositAmount)
                .With(t => t.Created, created)
                .With(t => t.Description, "Cash Deposit")
                .Create();

            Mock.Get(customer).Setup(c => c.Accounts).Returns(new List<IAccount> { customerAccount });

            Mock.Get(accountRepository)
                .Setup(a => a.GetGeneralLedgerCashAccount()).Returns(cashAccount);

            Mock.Get(transactionEngine)
                .Setup(tr => tr.CreateTransaction(cashAccount, customerAccount, depositAmount, "Cash Deposit"))
                .Returns(transaction);

            // The expected result is that the transaction has the cash account as left accout
            // the customer account as the right account and the correct amount 

            // TODO: Use autofixture create anonymous
            var accountOperationsManager = new AccountOperationsManager(
                transactionEngine, 
                transactionRepository, 
                accountRepository,
                customerRepository,
                investmentRepository,
                timeProvider);

            accountOperationsManager.Deposit(customer, customerAccount.AccountId, (double)depositAmount);

            Mock.Get(transactionRepository).Verify(r => r.AddTransaction(transaction));
            Mock.Get(transactionRepository).Verify(r => r.SaveChanges());
        }

        [TestMethod]
        public void TestWithdraw()
        {
            var fixture = new Fixture();

            var withdrawAmount = 100;
            var created = fixture.Freeze<DateTime>();

            var customer = Mock.Of<ICustomer>();

            var customerAccount =
                fixture
                .Build<Account>()
                .Without(a => a.Owners)
                .With(a => a.Balance, 150)
                .With(a => a.IsActive, true)
                .With(a => a.Category, AccountCategories.Liability)
                .Create();

            var cashAccount = fixture
                .Build<Account>()
                .Without(a => a.Owners)
                .With(a => a.Category, AccountCategories.Asset)
                .Create();

            // Parameters to the class under test
            var accountRepository = Mock.Of<IAccountRepository>();
            var transactionRepository = Mock.Of<ITransactionRepository>();
            var transactionEngine = Mock.Of<ITransactionEngine>();
            var customerRepository = Mock.Of<ICustomerRepository>();
            var investmentRepository = Mock.Of<IInvestmentRepository>();
            var timeProvider = Mock.Of<ITimeProvider>();

            // This is the resulting transaction that we expect to be saved to the repository
            var transaction =
                fixture
                .Build<Transaction>()
                .With(t => t.LeftAccount, customerAccount)
                .With(t => t.RightAccount, cashAccount)
                .With(t => t.Status, TransactionStatus.Pending)
                .With(t => t.Value, withdrawAmount)
                .With(t => t.Created, created)
                .With(t => t.Description, "Cash Withdraw")
                .Create();

            Mock.Get(customer).Setup(c => c.Accounts).Returns(new List<IAccount> { customerAccount });

            Mock.Get(accountRepository)
                .Setup(a => a.GetGeneralLedgerCashAccount()).Returns(cashAccount);

            Mock.Get(transactionEngine)
                .Setup(tr => tr.CreateTransaction(customerAccount, cashAccount, withdrawAmount, "Cash Withdraw"))
                .Returns(transaction);

            // The expected result is that the transaction has the cash account as right accout
            // the customer account as the left account and the correct amount 

            // TODO: Use autofixture create anonymous
            var accountOperationsManager = new AccountOperationsManager(
                transactionEngine, 
                transactionRepository,
                accountRepository,
                customerRepository,
                investmentRepository,
                timeProvider);

            accountOperationsManager.Withdraw(customer, customerAccount.AccountId, withdrawAmount);

            Mock.Get(transactionRepository).Verify(r => r.AddTransaction(transaction));
            Mock.Get(transactionRepository).Verify(r => r.SaveChanges());
        }

        [TestMethod]
        public void TestTransfer()
        {
            var fixture = new Fixture();

            // must be defined to compare to available balance
            var amount = 100;

            var created = fixture.Freeze<DateTime>();

            var customer = Mock.Of<ICustomer>();

            var sourceAccount =
                fixture
                .Build<Account>()
                .Without(a => a.Owners)
                .With(a => a.Balance, 150)
                .With(a => a.IsActive, true)
                .With(a => a.Category, AccountCategories.Liability)
                .Create();

            var destinationAccount = fixture.Build<Account>().Without(a => a.Owners).Create();

            // Parameters to the class under test
            var accountRepository = Mock.Of<IAccountRepository>();
            var transactionRepository = Mock.Of<ITransactionRepository>();
            var transactionEngine = Mock.Of<ITransactionEngine>();
            var customerRepository = Mock.Of<ICustomerRepository>();
            var investmentRepository = Mock.Of<IInvestmentRepository>();
            var timeProvider = Mock.Of<ITimeProvider>();

            // This is the resulting transaction that we expect to be saved to the repository
            var transaction =
                fixture
                .Build<Transaction>()
                .With(t => t.LeftAccount, destinationAccount)
                .With(t => t.RightAccount, sourceAccount)
                .With(t => t.Status, TransactionStatus.Pending)
                .With(t => t.Value, amount)
                .With(t => t.Created, created)
                .With(t => t.Description, "Online Transfer")
                .Create();

            Mock.Get(customer).Setup(c => c.Accounts).Returns(new List<IAccount> { sourceAccount, destinationAccount });

            Mock.Get(accountRepository)
                .Setup(a => a.GetGeneralLedgerCashAccount()).Returns(destinationAccount);

            Mock.Get(transactionEngine)
                .Setup(tr => tr.CreateTransaction(sourceAccount, destinationAccount, amount, "Online Transfer"))
                .Returns(transaction);

            Mock.Get(transactionRepository)
                .Setup(tr => tr.GetAccountTransactions(sourceAccount))
                .Returns(new List<ITransaction>());

            // The expected result is that the transaction has the source account as left accout
            // the destination account as the right account and the correct amount 

            // TODO: Use autofixture create anonymous
            var accountOperationsManager = new AccountOperationsManager(
                transactionEngine, 
                transactionRepository,
                accountRepository,
                customerRepository,
                investmentRepository,
                timeProvider);

            accountOperationsManager.Transfer(
                customer, sourceAccount.AccountId, destinationAccount.AccountId, amount);

            Mock.Get(transactionRepository).Verify(r => r.AddTransaction(transaction));
            Mock.Get(transactionRepository).Verify(r => r.SaveChanges());
        }

        [TestMethod]
        public void TestHasSufficientBalance()
        {
            // Assembly version issues with this one
            //var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var fixture = new Fixture();

            var mockAccount = fixture.Build<Account>().Without(a => a.Owners).Create();

            var sourceAccount = 
                fixture
                .Build<Account>()
                .With(a => a.Balance, 500)
                .With(a => a.Category, AccountCategories.Liability)
                .Without(a => a.Owners).Create();

            decimal amount = 300;

            var debitTransactions = 
                fixture
                .Build<Transaction>()
                .With(t => t.LeftAccount, sourceAccount)
                .With(t => t.RightAccount, mockAccount)
                .With(t => t.Status, TransactionStatus.Pending)
                .With(t => t.Value, 100)
                .CreateMany(3);

            var creditTransactions = 
                fixture
                .Build<Transaction>()
                .With(t => t.RightAccount, sourceAccount)
                .With(t => t.LeftAccount, mockAccount)
                .With(t => t.Status, TransactionStatus.Pending)
                .With(t => t.Value, 50)
                .CreateMany(3);

            var pendingTransactions = new List<ITransaction>();
            pendingTransactions.AddRange(debitTransactions);
            pendingTransactions.AddRange(creditTransactions);

            // Parameters to the class under test
            var accountRepository = Mock.Of<IAccountRepository>();
            var transactionRepository = Mock.Of<ITransactionRepository>();
            var transactionEngine = Mock.Of<ITransactionEngine>();
            var customerRepository = Mock.Of<ICustomerRepository>();
            var investmentRepository = Mock.Of<IInvestmentRepository>();
            var timeProvider = Mock.Of<ITimeProvider>();

            var accountOperationsManager = new AccountOperationsManager(
                transactionEngine, 
                transactionRepository, 
                accountRepository,
                customerRepository,
                investmentRepository,
                timeProvider);

            var obj = new PrivateObject(accountOperationsManager);
            var hasFunds = obj.Invoke("HasSufficientFunds", sourceAccount, amount, pendingTransactions);

            Assert.AreEqual(true, hasFunds);
        }
    }
}
