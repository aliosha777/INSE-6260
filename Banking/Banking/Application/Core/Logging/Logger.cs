namespace Banking.Application.Core.Logging
{
    using System;

    using Banking.Application.DAL;
    using Banking.Application.Models;

    public class Logger : ILogger
    {
        private readonly ILoggerRepository loggerRepository;

        public Logger(ILoggerRepository loggerRepository)
        {
            this.loggerRepository = loggerRepository;
        }

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
            var logEntry = new LogModel()
                {
                    Created = DateTime.Now,
                    ErrorMessage = ex.Message,
                    AdditionalData = ex.StackTrace
                };
            
            loggerRepository.AddLog(logEntry);
        }
    }
}