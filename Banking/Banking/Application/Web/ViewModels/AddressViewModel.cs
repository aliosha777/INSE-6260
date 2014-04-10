using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Application.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Address")]
    public class AddressViewModel
    {
        [Required]
        public string Line1 { get; set; }

        public string Line2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Province { get; set; }

        [Required]
        [RegularExpression(
            @"[ABCEFGHJKLMNPRSTVXY][0-9][ABCEFGHJKLMNPRSTVWXYZ] [\s]  [0-9][ABCEFGHJKLMNPRSTVWXYZ][0-9]", 
            ErrorMessage = "Invalid Postal Code")]
        public string PostalCode { get; set; }
    }
}