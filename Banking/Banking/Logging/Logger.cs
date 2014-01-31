using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Logging
{
    public class Logger : ILogger
    {
        public void Log(string message)
        {
            throw new NotImplementedException();
        }

        public void LogError(string errorMessage)
        {
            throw new NotImplementedException();
        }

        public void LogException(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}