using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Banking.Domain.Entities;

namespace Banking.Application.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    using Banking.Application.Models;

    public class TransactionViewModel
    {
        [Display(Name = "Transaction Id")]
        public int TransactionId { get; set; }

        [Display(Name = "Amount")]
        public decimal Value { get; set; }

        [Display(Name = "Created")]
        public DateTime Created { get; set; }

        [Display(Name = "Applied")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Applied { get; set; }
        
        [Display(Name = "Status")]
        public TransactionStatus Status { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public string Deposit { get; set; }

        public string Withdrawal { get; set; }

        [DisplayFormat(DataFormatString = ("{0:0.00}"))]
        public double AccountBalance { get; set; }
    }
}