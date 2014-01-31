using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banking.Models;

namespace Banking.DAL
{
    public interface IAccountRepository : IDisposable
    {
        Account GetAccountById(int accountId);

        void InsertAccount(Account account);

        void UpdateAccount(Account account);

        void Save();

        IEnumerable<Account> GetAllAccounts();
    }
}
