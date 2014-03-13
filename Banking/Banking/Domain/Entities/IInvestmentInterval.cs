namespace Banking.Domain.Entities
{
    using System;

    public interface IInvestmentInterval
    {
        DateTime Start { get; set; }

        DateTime End { get; set; }

        double InterestRate { get; set; }

        decimal StartingAmount { get; set; }
    }
}