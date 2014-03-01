﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Models
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

    //// Define IAccount interface to be able to have a general ledger account and a personal account
    //// yet be able to pass them to the transactional engine where the ownership of the account deos
    //// not matter 

    [Table("Account")]
    public class BankAccountModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }

        public string AccountNumber { get; set; }

        public AccountTypes Type { get; set; }

        public ICollection<CustomerModel> Owners { get; set; }

        public decimal Balance { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public bool IsActive { get; set; }

        public bool IsLocked { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}