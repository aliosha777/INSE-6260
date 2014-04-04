using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Application.Models
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
    public class TransactionModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        public BankAccountModel LeftAccount { get; set; }

        public BankAccountModel RightAccount { get; set; }

        public decimal Value { get; set; }

        public DateTime Created { get; set; }

        public DateTime Applied { get; set; }

        public TransactionStatus Status { get; set; }
    }
}