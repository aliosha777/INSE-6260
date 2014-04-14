namespace Banking.Domain.Entities
{
    using System;

    public interface IInvestmentInterval
    {
        int InvestmentIntervalId { get; set; }

        DateTime Start { get; set; }

        DateTime End { get; set; }

        double InterestRate { get; set; }

        decimal StartingAmount { get; set; }

        IInvestment Investment { get; set; }
    }
}