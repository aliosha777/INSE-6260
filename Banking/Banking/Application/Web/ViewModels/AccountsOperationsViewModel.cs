using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Banking.Domain.Entities;

namespace Banking.Application.Web.ViewModels
{
    using System.Web.Mvc;

    public enum OperationTypes
    {
        Deposit,
        Withdrawal,
        Transfer
    }

    public class AccountsOperationsViewModel
    {
        public AccountsOperationsViewModel()
        {
            AccountsSelectList = new List<SelectListItem>();
        }

        [Required]
        [Display(Name = "Amount")]
        public double Amount { get; set; }

        [Display(Name = "Select Source Account")]
        public IEnumerable<SelectListItem> SourceList
        {
            get
            {
                return AccountsSelectList;
            }

            set
            {
            }
        }

        [Display(Name = "Select Target Account ")]
        public IEnumerable<SelectListItem> TargetList
        {
            get
            {
                return AccountsSelectList;
            }

            set
            {
            }
        }

        public IEnumerable<SelectListItem> AccountsSelectList { get; set; }

        public int SelectedSourceAccountId { get; set; }

        public int SelectedTargetAccountId { get; set; }

        public OperationTypes OperationType { get; set; }
    }
}