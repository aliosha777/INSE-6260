using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Models
{
    public enum TransactionTypes
    {
        Credit,
        Debit,
        Transfer
    }

    public enum TransactionStatus
    {
        Applied,
        Pending,
        Failed
    }

    [Table("Transaction")]
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        public TransactionTypes Type { get; set; }

        public Account LeftAccount { get; set; }

        public Account RightAccount { get; set; }

        public decimal Value { get; set; }

        public DateTime Created { get; set; }

        public TransactionStatus Status { get; set; }
    }
}