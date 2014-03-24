using Banking.Domain.Entities;

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
        decimal CalculateProjectedBalanceAtMaturity(IInvestment investment);
    }
}