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