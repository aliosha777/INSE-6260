using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Application.Models
{
    [Table("InvestmentInterval")]
    public class InvestmentIntervalModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvestmentIntervalId { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public double InterestRate { get; set; }

        public decimal StartingAmount { get; set; }

        public int InvestmentId { get; set; }

        [ForeignKey("InvestmentId")]
        public virtual InvestmentModel Investment { get; set; }
    }
}