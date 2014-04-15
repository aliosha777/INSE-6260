using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Application.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class RequestStatementViewModel
    {
        [Required(ErrorMessage = "Statement start date is required")]
        [Display(Name = "From")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime From { get; set; }

        [Required(ErrorMessage = "Statement end date is required")]
        [Display(Name = "To")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime To { get; set; }

        public int AccountId { get; set; }
    }
}