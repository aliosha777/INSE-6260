namespace Banking.Tests.ApplicationLogic
{
    using System;
    using System.Collections.Generic;

    using Banking.Application.DAL;
    using Banking.Application.Models;
    using Banking.Domain.Entities;
    using Banking.Domain.Services.BankingOperationsEngine;
    using Banking.Exceptions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Ploeh.AutoFixture;

    [TestClass]
    public class TransactionEngineTests
    {
        [TestMethod]
        [ExpectedException(typeof(BankingValidationException))]
        public void TestCreateTransactionInactiveSourceAccount_ThrowsException()
        {
            var now = DateTime.Now;

            // Parameters to the class under test
            var accountRepository = Mock.Of<IAccountRepository>();
            var transactionRepository = Mock.Of<ITransactionRepository>();
            var timeProvider = Mock.Of<ITimeProvider>();

            Mock.Get(timeProvider).Setup(t => t.Now()).Returns(now);

            ITransactionEngine transactionEngine = 
                new TransactionEngine(transactionRepository, accountRepository, timeProvider);

            decimal amount = 1;

            var sourceAccount = Mock.Of<IAccount>();
            sourceAccount.IsActive = false;

            var destinationAccout = Mock.Of<IAccount>();
            destinationAccout.IsActive = true;

            transactionEngine.CreateTransaction(sourceAccount, destinationAccout, amount);
        }

        [TestMethod]
        [ExpectedException(typeof(BankingValidationException))]
        public void TestCreateTransactionInactiveDestinationAccount_ThrowsException()
        {
            var now = DateTime.Now;

            // Parameters to the class under test
            var accountRepository = Mock.Of<IAccountRepository>();
            var transactionRepository = Mock.Of<ITransactionRepository>();
            var timeProvider = Mock.Of<ITimeProvider>();

            Mock.Get(timeProvider).Setup(t => t.Now()).Returns(now);

            ITransactionEngine transactionEngine =
                new TransactionEngine(transactionRepository, accountRepository, timeProvider);

            decimal amount = 1;

            var leftAccount = Mock.Of<IAccount>();
            leftAccount.IsActive = true;

            var rightAccount = Mock.Of<IAccount>();
            rightAccount.IsActive = false;

            transactionEngine.CreateTransaction(leftAccount, rightAccount, amount);
        }

        [TestMethod]
        public void TestCreateTransaction_Success()
        {
            var fixture = new Fixture();
            var now = DateTime.Now;

            var accountRepository = Mock.Of<IAccountRepository>();
            var transactionRepository = Mock.Of<ITransactionRepository>();
            var timeProvider = Mock.Of<ITimeProvider>();

            Mock.Get(timeProvider).Setup(t => t.Now()).Returns(now);

            ITransactionEngine transactionEngine =
                new TransactionEngine(transactionRepository, accountRepository, timeProvider);

            var leftAccount = 
                fixture
                .Build<Account>()
                .Without(a => a.Owners)
                .With(a => a.IsActive, true)
                .Create();

            var rightAccount =
                fixture
                .Build<Account>()
                .Without(a => a.Owners)
                .With(a => a.IsActive, true)
                .Create();

            decimal amount = 100;

            var transaction = transactionEngine.CreateTransaction(leftAccount, rightAccount, amount);

            Assert.AreEqual(transaction.Status, TransactionStatus.Pending);
            Assert.AreEqual(transaction.LeftAccount, leftAccount);
            Assert.AreEqual(transaction.RightAccount, rightAccount);
            Assert.AreEqual(transaction.Value, amount);
        }

        [TestMethod]
        public void TestApplyTransactionNonPending_ThrowsException()
        {
            /*
            var fixture = new Fixture().Customize(new BankingCustomization());

            var account = fixture.Build<Account>().Create();

            Console.WriteLine(account.AccountId);

            var transaction = fixture.Build<Transaction>().Create();

            Console.WriteLine(transaction.TransactionId);
             */
        }

        [TestMethod]
        public void TestApplyTransaction_Success()
        {
            var fixture = new Fixture();
            var transactionRepository = Mock.Of<ITransactionRepository>();
            var accountRepository = Mock.Of<IAccountRepository>();
            var timeProvider = Mock.Of<ITimeProvider>();

            var today = DateTime.Now;
            Mock.Get(timeProvider).Setup(t => t.Now()).Returns(today);
            

            var leftAccount =
                fixture
                .Build<Account>()
                .With(a => a.Balance, 500)
                .With(a => a.Category, AccountCategories.Asset)
                .Without(a => a.Owners).Create();

            var rightAccount =
               fixture
               .Build<Account>()
               .With(a => a.Balance, 500)
               .With(a => a.Category, AccountCategories.Liability)
               .Without(a => a.Owners).Create();

            var transaction =
               fixture
               .Build<Transaction>()
               .With(t => t.LeftAccount, leftAccount)
               .With(t => t.RightAccount, rightAccount)
               .With(t => t.Status, TransactionStatus.Pending)
               .With(t => t.Value, 100)
               .With(t => t.Applied, today)
               .Create();

            ITransactionEngine transactionEngine = 
                new TransactionEngine(transactionRepository, accountRepository, timeProvider);

            transactionEngine.ApplyTransaction(transaction);

            Assert.AreEqual(transaction.Status, TransactionStatus.Applied);

            Assert.AreEqual(600, leftAccount.Balance);

            Assert.AreEqual(600, rightAccount.Balance);

            Mock.Get(accountRepository).Verify(ar => ar.UpdateAccount(leftAccount, false));
            Mock.Get(accountRepository).Verify(ar => ar.UpdateAccount(rightAccount, false));
            Mock.Get(accountRepository).Verify(ar => ar.Save());

            Mock.Get(transactionRepository).Verify(tr => tr.UpdateTransaction(transaction));
            Mock.Get(transactionRepository).Verify(tr => tr.SaveChanges());
        }
    } 
}
