using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Banking.BankingOperationsEngine;
using Banking.DAL;
using Banking.Exceptions;
using Banking.ViewModels;

namespace Banking.Controllers
{
    [Authorize(Roles = "Teller")]
    public class TellerController : Controller
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ICustomerOperationsManager customerOperationsManager;

        public TellerController(
            ICustomerRepository customerRepository,
            ICustomerOperationsManager customerOperationsManager)
        {
            this.customerRepository = customerRepository;
            this.customerOperationsManager = customerOperationsManager;
        }

        // GET: /Teller/

        public ActionResult Home()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CustomerSummary(string customerId)
        {
            int id;

            CustomerSummary customerSummary = null;

            if (int.TryParse(customerId, out id))
            {
                var customer = customerRepository.GetCustomerById(id);
                customerSummary = new CustomerSummary();

                foreach (var account in customer.Accounts)
                {
                    customerSummary.Accounts.Add(account);
                }

                var address = customerOperationsManager.GetActiveAddress(customer);

                customerSummary.CurrentAddress.Line1 = address.Line1;
                customerSummary.CurrentAddress.Line2 = address.Line2;
                customerSummary.CurrentAddress.City = address.City;
                customerSummary.CurrentAddress.Province = address.Province;
                customerSummary.CurrentAddress.PostalCode = address.PostalCode;
            }

            return View(customerSummary);
        }
    }
}
