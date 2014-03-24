using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Application.DAL
{
    using Banking.Domain.Entities;

    public class InvestmentRepository : IInvestmentRepository
    {
        private BankDBContext context;
        private bool disposed = false;

        public InvestmentRepository(BankDBContext context)
        {
            this.context = context;
        }

        public IInvestment GetInvestment(int investmentId)
        {
            return null;
        }

        public void UpdateInvestment(Investment investment)
        {
        }

        public void Save()
        {
            context.SaveChanges();
        }

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
    }
}