using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Application.Web.ViewModels
{
    public class CustomerPersonalInformation
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public AddressViewModel Address { get; set; }
    }
}