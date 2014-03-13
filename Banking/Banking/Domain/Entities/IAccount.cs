using Banking.Models;
using System;
using System.Collections.Generic;

namespace Banking.Domain.Entities
{
    // TODO: Make those enums into database lookup tables
    public enum AccountCategories
    {
        Asset,
        Liability
    }

    public enum AccountTypes
    {
        Checking,
        Savings,
        Investment,
        //// For simplicity we could use the same account class for general ledger accounts 
        //// but might be better to define it as a separate entity
        GeneralLedgerCash,
    }

    public interface IAccount
    {
        int AccountId { get; set; }

        string AccountNumber { get; set; }

        AccountTypes Type { get; set; }

        AccountCategories Category { get; set; }

        ICollection<ICustomer> Owners { get; set; }

        decimal Balance { get; set; }

        DateTime Created { get; set; }

        DateTime Modified { get; set; }

        bool IsActive { get; set; }

        bool IsLocked { get; set; }
    }
}
