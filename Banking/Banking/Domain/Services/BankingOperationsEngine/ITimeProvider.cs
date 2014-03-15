using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.BankingOperationsEngine
{
    public interface ITimeProvider
    {
        DateTime GetNextBusinessDay();

        DateTime GetQuarterSince(DateTime date);

        int GetDifferenceInYears(DateTime start, DateTime end);

        DateTime Today();
    }
}
