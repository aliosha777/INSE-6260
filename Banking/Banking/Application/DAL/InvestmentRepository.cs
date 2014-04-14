using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Application.DAL
{
    using System.Data;

    using Banking.Domain.Core;
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
            var investment = 
                context
                .Investments
                .Include("InvestmentIntervals")
                .SingleOrDefault(i => i.InvestmentId == investmentId);

            return investment.ToInvestment();
        }

        public void AddInvestment(IInvestment investment)
        {
            var investmentModel = investment.ToModel();

            context.Investments.Add(investmentModel);
            context.SaveChanges();
            investment.InvestmentId = investmentModel.InvestmentId;
        }

        public IEnumerable<IInvestment> GetAccountInvestments(int accountId)
        {
            var investments = 
                context
                .Investments
                .Include("InvestmentIntervals")
                .Where(i => i.AccountId == accountId).ToList();

            return investments.Select(i => i.ToInvestment());
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