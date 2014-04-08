namespace Banking.Application.Core.Logging
{
    using System;

    public interface ILogger
    {
        void Log(string message);

        void LogError(string errorMessage);

        void LogException(Exception ex);
    }
}
