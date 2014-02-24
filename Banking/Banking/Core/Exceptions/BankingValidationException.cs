using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Exceptions
{
    public class BankingValidationException : Exception
    {
        public BankingValidationException()
            : base()
        {
        }

        public BankingValidationException(string message)
            : base(message)
        {
        }

        public BankingValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}