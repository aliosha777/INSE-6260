using System;

using Banking.Domain.Entities;

namespace Banking.Application.DAL
{
    public interface ICustomerRepository : IDisposable
    {
        ICustomer GetCustomerById(int customerId);

        void AddCustomer(ICustomer customer);

        void Save();
    }
}
