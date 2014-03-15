using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.BankingOperationsEngine
{
    using Banking.Domain.Entities;

    public class InvestmentManager : IInvestmentManager
    {
        private readonly ITimeProvider timeProvider;

        public InvestmentManager(ITimeProvider timeProvider)
        {
            this.timeProvider = timeProvider;
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
        public decimal CalculateProjectedBalanceAtMaturity(IInvestment investment)
        {
            // Assumptions: investment term is in whole years
            var interval = investment.InvestmentIntervals.FirstOrDefault();

            var yearCount = timeProvider.GetDifferenceInYears(interval.Start, interval.End);

            var amount = ((double)interval.StartingAmount) * Math.Pow(1 + interval.InterestRate, yearCount);

            return (decimal)amount;
        }

        public DateTime GetNextCompoundingDate(IInvestment investment)
        {
            // Count how many compounding periods passed from the beginniNg of the investment
            // then return the last incomplete period

            // for simplicity we will use days/365 for the fractional periods and there will 
            // be no compensation for leap years

            if (investment.CompoundingFrequency == CompoundingFrequency.Yearly)
            {
                var yearsFromStart = timeProvider.GetDifferenceInYears(investment.TermStart, timeProvider.Today());
            }

            return new DateTime();
        }

        private decimal CalculateInterestOfInterval(IInvestmentInterval investmentInterval)
        {
            decimal interest = 0;

            return interest;
        }
    }
}