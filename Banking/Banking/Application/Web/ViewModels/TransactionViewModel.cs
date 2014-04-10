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
        public DateTime? Applied { get; set; }
        
        [Display(Name = "Status")]
        public TransactionStatus Status { get; set; }
    }
}