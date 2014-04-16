using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Banking.Domain.Entities;

namespace Banking.Domain.Services.AdminOperations
{
    public interface ICustomerManager
    {
        IEnumerable<ICustomer> FindCustomerByFirstName(string firstName);

       ICustomer FindCustomerByUsername(string userName);

        IEnumerable<ICustomer> FindCustomerByAccountNumber(string accountNumber);
    }
}