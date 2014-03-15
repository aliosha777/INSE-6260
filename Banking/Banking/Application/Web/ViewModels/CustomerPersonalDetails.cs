using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.ViewModels
{
    using Banking.Domain.Entities;
    using Banking.Models;

    public class CustomerPersonalDetails
    {
        public IAddress Address { get; set; }
    }
}