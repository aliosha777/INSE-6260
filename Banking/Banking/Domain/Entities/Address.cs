using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Domain.Entities
{
    public class Address : IAddress
    {
        public string AddressId { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string PostalCode { get; set; }

        public bool IsActive { get; set; } 
    }
}