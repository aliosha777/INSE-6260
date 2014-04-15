using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Banking.Domain.Entities;

namespace Banking.Application.Web.ViewModels
{
    public class InvestmentViewModel
    {
        public InvestmentViewModel()
        {
            InvesementTypes = new List<SelectListItem>();
        }

        public InvestmentTypes Type { get; set; }

        public CompoundingFrequency Frequency { get; set; }

        public int MaxYears { get; set; }

        public IEnumerable<SelectListItem> TermList
        {
            get
            {
                for (int i = 1; i <= MaxYears; i++)
                {
                    yield return new SelectListItem { Value = i.ToString(), Text = i.ToString() };
                }
            }
        }

        public IEnumerable<SelectListItem> AccountsList { get; set; }

        public int AccountId { get; set; }

        [Display(Name = "Interest Amount")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double InterestAmount { get; set; }

        [Display(Name = "InterestDue")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime InterestDueDate { get; set; }

        [Display(Name = "Interest")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double Interest { get; set; }

        [Display(Name = "Investment Term in years")]
        public int TermDuration { get; set; }

        [Display(Name = "Term Start")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Start { get; set; }

        [Display(Name = "Term End")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime End { get; set; }

        [Display(Name = "Interest Rate")]
        [DisplayFormat(DataFormatString = "{0:P1}")]
        public double Rate { get; set; }

        [Display(Name = "Investment Amount")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public double StartingAmount { get; set; }

        [Display(Name = "Select Investment Type")]
        public IEnumerable<SelectListItem> InvesementTypes { get; set; }

        [Display(Name = "Selct Compounding Frequency")]
        public IEnumerable<SelectListItem> CompoundingFrequecyList { get; set; }
    }
}