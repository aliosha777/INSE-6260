using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Models
{
    [Table("Account")]
    public class Account
    {
        public enum AccountTypes
        {
            Checking,
            Savings,
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }

        public string AccountNumber { get; set; }

        public AccountTypes Type { get; set; }

        public ICollection<Customer> Owners { get; private set; }

        public decimal Balance { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public bool IsActive { get; set; }
    }
}