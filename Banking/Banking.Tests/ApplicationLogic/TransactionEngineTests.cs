namespace Banking.Tests.ApplicationLogic
{
    using System;
    using System.Collections.Generic;

    using Banking.BankingOperationsEngine;
    using Banking.Domain.Entities;
    using Banking.Exceptions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class TransactionEngineTests
    {
        [TestMethod]
        [ExpectedException(typeof(BankingValidationException))]
        public void TestTransactionNonLockedSourceAccount_ThrowsException()
        {
            ITransactionEngine transactionEngine = new TransactionEngine();

            decimal amount = 1;

            var sourceAccount = Mock.Of<IAccount>();
            sourceAccount.IsActive = false;

            var destinationAccout = Mock.Of<IAccount>();
            destinationAccout.IsActive = true;

            var pendingTransactions = new List<ITransaction>();
        }

        [TestMethod]
        [ExpectedException(typeof(BankingValidationException))]
        public void TestTransactionNonLockedDestinationAccount_ThrowsException()
        {
            ITransactionEngine transactionEngine = new TransactionEngine();

            decimal amount = 1;

            var sourceAccount = Mock.Of<IAccount>();
            sourceAccount.IsActive = true;

            var destinationAccout = Mock.Of<IAccount>();
            destinationAccout.IsActive = false;

            var pendingTransactions = new List<ITransaction>();
        }

        [TestMethod]
        public void TestCalculatePendingAmounts()
        {
            var pendingTransactions = new List<ITransaction>();

            var sourceAccount = Mock.Of<IAccount>();
            sourceAccount.IsLocked = true;

            var destinationAccout = Mock.Of<IAccount>();
            destinationAccout.IsLocked = true;

            decimal amount = 10;

            var pendingTransaction1 = Mock.Of<ITransaction>();
            pendingTransaction1.Value = 1;

            pendingTransactions.Add(pendingTransaction1);

            //var obj = new PrivateObject(new TransactionEngine());
            //obj.Invoke("HasSufficientFunds");

            ITransactionEngine transactionEngine = new TransactionEngine();

        }

        [TestMethod]
        public void TestTransferWithInsufficientFunds_TransactionWithFailedStatus()
        {
        }
    } 
}
