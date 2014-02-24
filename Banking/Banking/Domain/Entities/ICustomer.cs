using System.Collections.Generic;

using Banking.Models;

namespace Banking.Domain.Entities
{
    public interface ICustomer
    {
       int CustomerId { get; set; }

       string FirstName { get; set; }

       string LastName { get; set; }

       string Phone { get; set; }

       string Email { get; set; }

       List<IAddress> Addresses { get; set; }

       List<IAccount> Accounts { get; set; }
    }
}
