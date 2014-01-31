using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.DAL
{
    using Banking.Models;

    public interface ICustomerRepository
    {
        Customer GetCustomerById(int customerId);
    }
}
