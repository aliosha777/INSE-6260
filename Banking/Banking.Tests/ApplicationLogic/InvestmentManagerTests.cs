﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Banking.Domain.Entities;
using Banking.Domain.Services.BankingOperationsEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;

namespace Banking.Tests.ApplicationLogic
{
    [TestClass]
    public class InvestmentManagerTests
    {
        [TestMethod]
        public void TestCalculateBalanceAtMaturityFixedRate()
        {
            var fixture = new Fixture();

            var start = new DateTime(2014, 1, 1);
            var end = new DateTime(2016, 1, 1);

            decimal startingAmount = 1000;

            var investmentInterval = 
                fixture
                .Build<InvestmentInterval>()
                .With(i => i.Start, start)
                .With(i => i.End, end)
                .With(i => i.InterestRate, 0.05)
                .With(i => i.StartingAmount, startingAmount)
                .Create();

            var intervals = new List<IInvestmentInterval>();
            intervals.Add(investmentInterval);

            IInvestment investment = 
                fixture
                .Build<Investment>()
                .With(i => i.TermStart, start)
                .With(i => i.TermEnd, end)
                .With(i => i.Type, InvestmentTypes.FixedRate)
                .With(i => i.CompoundingFrequency, CompoundingFrequency.Yearly )
                .With(i => i.InvestmentIntervals, intervals)
                .Create();

            var timeProvider = Mock.Of<ITimeProvider>();

            Mock.Get(timeProvider).Setup(t => t.GetDifferenceInYears(start, end)).Returns(2);

            var investmentManager = new InvestmentManager(timeProvider);

            var balance = investmentManager.CalculateProjectedBalanceAtMaturity(investment);

            Assert.AreEqual(1102.5M, balance);
        }
    }
}
