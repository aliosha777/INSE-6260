namespace Banking.DAL
{
    using System;

    using Banking.Domain.Entities;

    public interface IInvestmentRepository : IDisposable
    {
        IInvestment GetInvestment(int investmentId);

        void UpdateInvestment(Investment investment);

        void Save();
    }
}