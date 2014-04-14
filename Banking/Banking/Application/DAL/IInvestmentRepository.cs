using System;

using Banking.Domain.Entities;

namespace Banking.Application.DAL
{
    using System.Collections.Generic;

    public interface IInvestmentRepository : IDisposable
    {
        IInvestment GetInvestment(int investmentId);

        void UpdateInvestment(Investment investment);

        void Save();

        void AddInvestment(IInvestment investment);

        IEnumerable<IInvestment >GetAccountInvestments(int accountId);
    }
}