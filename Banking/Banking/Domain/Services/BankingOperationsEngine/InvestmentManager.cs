using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Banking.Domain.Entities;

namespace Banking.Domain.Services.BankingOperationsEngine
{
    using Banking.Application.DAL;
    using Banking.Exceptions;

    public class InvestmentManager : IInvestmentManager
    {
        private const double FixedInterestRate = 0.05;
        private const int MaxTermInYears = 5;

        private readonly ITimeProvider timeProvider;
        private readonly IInvestmentRepository investmentRepository;

        public InvestmentManager(
            ITimeProvider timeProvider,
            IInvestmentRepository investmentRepository)
        {
            this.timeProvider = timeProvider;
            this.investmentRepository = investmentRepository;
        }

        public Investment CreateGicInvestment(
            DateTime start, int termDuration, double startingAmount, IAccount associatedAccount)
        {
            if (associatedAccount.Type != AccountTypes.Investment)
            {
                throw new BankingValidationException("Investments can only be associated with accounts of type Investment.");
            }

            var termEnd = start.AddYears(termDuration);

            var investment = new Investment
            {
                Type = InvestmentTypes.FixedRate,
                TermStart = start,
                TermEnd = termEnd,
                CompoundingFrequency = CompoundingFrequency.Yearly,
                Account = associatedAccount
            };

            var investmentInterval = new InvestmentInterval()
                {
                    Start = start,
                    End = termEnd,
                    InterestRate = this.GetGicInterestrate(),
                    StartingAmount = (decimal)startingAmount,
                    Investment = investment
                };

            investment.InvestmentIntervals.Add(investmentInterval);

            investmentRepository.AddInvestment(investment);

            return investment;
        }

        // Investment account: regular account + interest calculator

        // Functionaliy needed: at regular intervals get the accumulated interest to an account

        /// <summary>
        /// For simplicity this method does not compensate for day fractions
        /// </summary>
        /// <param name="investment"></param>
        /// <param name="untilDate"></param>
        /// <returns></returns>
        public decimal CalculateCompoundInterestToDate(IInvestment investment, DateTime untilDate)
        {
            // Compound Interest formula with yearly compounding: M = P x (1 + i)^n

            /* General compound interest formula
                A = P(1 + r/n)^nt
              
                P = principal amount (the initial amount you borrow or deposit)
                r = annual rate of interest (as a decimal)
                t = number of years the amount is deposited or borrowed for.
                A = amount of money accumulated after n years, including interest.
                n = number of times the interest is compounded per year 
             */

            decimal balance;

            foreach (var investmentPeriod in investment.InvestmentIntervals)
            {
                var endDate = investmentPeriod.End <= untilDate ? investmentPeriod.End : untilDate;

                var daysCount = endDate.Subtract(investmentPeriod.Start).Days;

                var amount = investmentPeriod.StartingAmount;

                var interestRate = investmentPeriod.InterestRate;

                double intermediate = 1 + (interestRate / (double)investment.CompoundingFrequency);

                var investmentTerm = (investment.TermEnd.Subtract(investment.TermStart)).Days / 365;

                double interest = Math.Pow(intermediate, investmentTerm);
            }

            return 0;
        }

        /// <summary>
        /// Calculates the balance of a fixed term investment at the end of the term provided 
        /// no changes are made to the investment.
        /// </summary>
        /// <param name="investment"></param>
        /// <returns></returns>
        public decimal CalculateBalanceAtMaturity(IInvestment investment)
        {
            // Assumptions: investment term is in whole years
            var interval = investment.InvestmentIntervals.FirstOrDefault();

            var yearCount = timeProvider.GetDifferenceInYears(interval.Start, interval.End);

            var amount = CalculateFixedRateCompoundInterest(
                (double)interval.StartingAmount, interval.InterestRate, yearCount);

            return (decimal)amount;
        }

        public double CalculateProjectedInterestAtMaturity(int termDuration, double startingAmount, double interestRate)
        {
            return CalculateFixedRateCompoundInterest(startingAmount, interestRate, termDuration);
        }

        public DateTime GetNextCompoundingDate(IInvestment investment)
        {
            // Count how many compounding periods passed from the beginniNg of the investment
            // then return the last incomplete period

            // for simplicity we will use days/365 for the fractional periods and there will 
            // be no compensation for leap years

            if (investment.CompoundingFrequency == CompoundingFrequency.Yearly)
            {
                var yearsFromStart = timeProvider.GetDifferenceInYears(investment.TermStart, timeProvider.Now());
            }

            return timeProvider.Now();
        }

        public double GetGicInterestrate()
        {
            return FixedInterestRate;
        }

        public int GetMaxTermInYears()
        {
            return MaxTermInYears;
        }

        /// <summary>
        /// Calculates the compound interest on the given amount assuming yearly compounding.
        /// Can be used to calculate the interest at the end of an investment interval
        /// </summary>
        /// <param name="startingAmount"></param>
        /// <param name="interestRate"></param>
        /// <param name="years"></param>
        /// <returns></returns>
        private double CalculateFixedRateCompoundInterest(double startingAmount, double interestRate, int years)
        {
            double amount = startingAmount * Math.Pow(1 + interestRate, years);
            return amount;
        }
    }
}