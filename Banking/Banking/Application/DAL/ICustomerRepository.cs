using System;

using Banking.Domain.Entities;

namespace Banking.Application.DAL
{
    using System.Collections.Generic;

    public interface ICustomerRepository : IDisposable
    {
        ICustomer GetCustomerById(int customerId);

        void AddCustomer(ICustomer customer);

        void Save();

        ICustomer UpdateCustomer(ICustomer customer, bool saveImmediately);

        IEnumerable<ICustomer> GetCustomersByFirstName(string firstName);

        IEnumerable<ICustomer> GetCustomersByUserName(string userName);

        IEnumerable<ICustomer> GetCustomersByAccountNumber(string accountNumber);
    }
}
