using Banking.Models;

namespace Banking.Domain.Entities
{
    public class Account : IAccount
    {
        public Account(Banking.Models.Account account)
        {
            this.AccountId = account.AccountId;
            this.AccountNumber = account.AccountNumber;
            this.Balance = account.Balance;
            this.IsActive = account.IsActive;
            this.IsLocked = account.IsLocked;
            this.Type = account.Type;
        }

        public int AccountId { get; set; }

        public string AccountNumber { get; set; }

        public AccountTypes Type { get; set; }

        public decimal Balance { get; set; }

        public bool IsActive { get; set; }

        public bool IsLocked { get; set; }
    }
}