namespace Banking.Domain.Entities
{
    using System;
    using System.Collections.Generic;

    public enum InvestmentTypes
    {
        FixedRate,
        VariableRate
    }

    public enum CompoundingFrequency
    {
        Yearly,
        Quarterly
    }

    public interface IInvestment
    {
        DateTime TermStart { get; set; }

        DateTime TermEnd { get; set; }

        InvestmentTypes Type { get; set; }

        CompoundingFrequency CompoundingFrequency { get; set; }

        List<IInvestmentInterval> InvestmentIntervals { get; set; }
    }
}