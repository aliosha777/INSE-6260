using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Domain.Services.BankingOperationsEngine
{
    public class TimeProvider : ITimeProvider
    {
        private const int DaysInYear = 365;

        private const int DaysInQuarter = 91;

        public DateTime GetNextBusinessDay()
        {
            throw new NotImplementedException();
        }

        public DateTime GetQuarterSince(DateTime date)
        {
            return date.AddDays(DaysInQuarter);
        }

        public int GetDifferenceInYears(DateTime start, DateTime end)
        {
            // TODO: adjust for leap years
            return end.Subtract(start).Days / DaysInYear;
        }

        public DateTime Now()
        {
            return DateTime.Today;
        }
    }
}