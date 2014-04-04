using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Application.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Address")]
    public class AddressViewModel
    {
        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string PostalCode { get; set; }
    }
}