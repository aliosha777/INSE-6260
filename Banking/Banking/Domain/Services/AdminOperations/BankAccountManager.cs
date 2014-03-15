using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.AdminOperations
{
    public class BankAccountManager
    {
        private const string BaseAccountNumber = "0000-003-";

        private const string BaseBranchNumber = "";

        public string GetNewAccountNumber()
        {
            // This is a quick hack, should get the next incremental account number from the database
            var random = new Random();

            var accountNumber = random.Next(10000, 99999);
            return BaseAccountNumber + accountNumber;
        }
    }
}