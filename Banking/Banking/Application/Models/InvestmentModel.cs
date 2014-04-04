using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Banking.Domain.Entities;

namespace Banking.Application.Models
{
    [Table("Investment")]
    public class InvestmentModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvestmentId { get; set; }

        public int AccountId { get; set; }

        [ForeignKey("AccountId")]
        public BankAccountModel Account { get; set; }

        public DateTime TermStart { get; set; }

        public DateTime TermEnd { get; set; }

        public InvestmentTypes Type { get; set; }

        public CompoundingFrequency CompoundingFrequency { get; set; }

        public List<InvestmentIntervalModel> InvestmentIntervals { get; set; } 
    }
}