using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Domain.Entities
{
    /// <summary>
    /// To track interest calculations when conditions change like funds added or withdrawn 
    /// from the investment or interest rate changing
    /// </summary>
    public class InvestmentInterval : IInvestmentInterval
    {
        public int InvestmentIntervalId { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public double InterestRate { get; set; }

        public decimal StartingAmount { get; set; }

        public IInvestment Investment { get; set; }
    }
}