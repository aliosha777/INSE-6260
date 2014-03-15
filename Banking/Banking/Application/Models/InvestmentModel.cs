using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Banking.Domain.Entities;

    [Table("Investment")]
    public class InvestmentModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvestmentId { get; set; }

        public BankAccountModel Account { get; set; }

        public DateTime TermStart { get; set; }

        public DateTime TermEnd { get; set; }

        public InvestmentTypes Type { get; set; }

        public CompoundingFrequency CompoundingFrequency { get; set; }

        public ICollection<InvestmentIntervalModel> InvestmentIntervals { get; set; } 
    }
}