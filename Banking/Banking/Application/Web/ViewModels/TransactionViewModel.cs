using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Banking.Domain.Entities;

namespace Banking.Application.Web.ViewModels
{
    using Banking.Application.Models;

    public class TransactionViewModel
    {
        public int TransactionId { get; set; }

        public decimal Value { get; set; }

        public DateTime Created { get; set; }

        public DateTime Applied { get; set; }

        public TransactionStatus Status { get; set; }
    }
}