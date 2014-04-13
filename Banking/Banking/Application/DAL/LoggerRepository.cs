using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Application.DAL
{
    using Banking.Application.Models;

    public interface ILoggerRepository : IDisposable
    {
        void AddLog(LogModel log);

        IEnumerable<LogModel> GetLogs(int numberOfEntries);
    }

    public class LoggerRepository : ILoggerRepository
    {
        private bool disposed;
        private BankDBContext context;

        public LoggerRepository()
            : this(new BankDBContext())
        {
        }

        public LoggerRepository(BankDBContext context)
        {
            this.context = context;
        }

        public void AddLog(LogModel log)
        {
            context.Logs.Add(log);
            context.SaveChanges();
        }

        public IEnumerable<LogModel> GetLogs(int numberOfEntries)
        {
            var logs = context.Logs.OrderByDescending(l => l.LogId).Take(numberOfEntries);

            return logs.ToList();
        }

        #region IDisposable
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }

            this.disposed = true;
        }

        #endregion
    }
}