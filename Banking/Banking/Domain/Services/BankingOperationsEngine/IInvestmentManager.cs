using Banking.Domain.Entities;
using System;
namespace Banking.Domain.Services.BankingOperationsEngine
{
    public interface IInvestmentManager
    {
        /// <summary>
        /// Calculates the balance of a fixed term investment at the end of the term provided 
        /// no changes are made to the investment.
        /// </summary>
        /// <param name="investment"></param>
        /// <returns></returns>
        decimal CalculateBalanceAtMaturity(IInvestment investment);

        double GetGicInterestrate();

        int GetMaxTermInYears();

        double CalculateProjectedInterestAtMaturity(int termDuration, double startingAmount, double interestRate);

        Investment CreateGicInvestment(DateTime start, int termDuration, double startingAmount, IAccount associatedAccount);
    }
}