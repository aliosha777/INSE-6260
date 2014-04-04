using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Banking.Domain.Entities;

namespace Banking.Application.Models
{
    [Table("Account")]
    public class BankAccountModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }

        public string AccountNumber { get; set; }

        public AccountTypes Type { get; set; }

        public AccountCategories Category { get; set; }

        public List<CustomerModel> Owners { get; set; }

        public decimal Balance { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public bool IsActive { get; set; }

        public bool IsLocked { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}