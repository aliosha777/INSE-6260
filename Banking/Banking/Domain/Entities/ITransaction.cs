using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banking.Models;

namespace Banking.Domain.Entities
{
    public interface ITransaction
    {
        int TransactionId { get; set; }

        IAccount LeftAccount { get; set; }

        IAccount RightAccount { get; set; }

        decimal Value { get; set; }

        DateTime Created { get; set; }

        TransactionStatus Status { get; set; }
    }
}
