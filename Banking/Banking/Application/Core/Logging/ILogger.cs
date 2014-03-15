using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Logging
{
    public interface ILogger
    {
        void Log(string message);

        void LogError(string errorMessage);

        void LogException(Exception ex);
    }
}
