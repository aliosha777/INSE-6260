using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Domain.Entities
{
    public class Investment : IInvestment
    {
        public Investment()
        {
            InvestmentIntervals = new List<IInvestmentInterval>();
        }

        public int InvestmentId { get; set; }

        public IAccount Account { get; set; }

        public DateTime TermStart { get; set; }

        public DateTime TermEnd { get; set; }

        public InvestmentTypes Type { get; set; }

        public CompoundingFrequency CompoundingFrequency { get; set; }

        public List<IInvestmentInterval> InvestmentIntervals { get; set; }
    }
}