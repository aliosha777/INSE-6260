using System;

using Banking.Domain.Entities;

namespace Banking.DAL
{
    public interface ICustomerRepository : IDisposable
    {
        ICustomer GetCustomerById(int customerId);
    }
}
