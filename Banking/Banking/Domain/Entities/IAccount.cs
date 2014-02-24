using Banking.Models;
using System;
using System.Collections.Generic;

namespace Banking.Domain.Entities
{
    public interface IAccount
    {
        int AccountId { get; set; }

        string AccountNumber { get; set; }

        AccountTypes Type { get; set; }

        ICollection<ICustomer> Owners { get; set; }

        decimal Balance { get; set; }

        DateTime Created { get; set; }

        DateTime Modified { get; set; }

        bool IsActive { get; set; }

        bool IsLocked { get; set; }
    }
}
