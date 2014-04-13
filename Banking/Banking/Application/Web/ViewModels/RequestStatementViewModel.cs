using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Application.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class RequestStatementViewModel
    {
        [Required]
        [Display(Name = "From")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime From { get; set; }

        [Required]
        [Display(Name = "To")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime To { get; set; }

        public int AccountId { get; set; }
    }
}