using Banking.Models;

namespace Banking.Domain.Entities
{
    public interface IAccount
    {
        int AccountId { get; set; }

        string AccountNumber { get; set; }

        AccountTypes Type { get; set; }

        decimal Balance { get; set; }

        bool IsActive { get; set; }

        bool IsLocked { get; set; }
    }
}
