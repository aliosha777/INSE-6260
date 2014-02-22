using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Banking.Controllers
{
    using Banking.DAL;
    using Banking.ViewModels;

    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        // GET: /Customer/
        
        public ActionResult CustomerSummary(int id)
        {
            var customer = customerRepository.GetCustomerById(id);

            var customerSummary = new CustomerSummary
            {
                Accounts = customer.Accounts,
                FirstName = customer.FirstName,
                LastName = customer.LastName
            };

            return View(customerSummary);
        }

        public ActionResult PersonalDetails(int id)
        {
            var customer = customerRepository.GetCustomerById(id);

            var customerPersonalDetails = new CustomerPersonalDetails
            {
                Address = customer.Addresses.FirstOrDefault(a => a.IsActive)
            };

            return View(customerPersonalDetails);
        }
    }
}
