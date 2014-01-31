using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Banking.Controllers
{
    using Banking.DAL;
    using Banking.ViewModels;

    [Authorize(Roles = "Teller")]
    public class TellerController : Controller
    {
        private readonly ICustomerRepository customerRepository;

        public TellerController(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
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
                var cust = customerRepository.GetCustomerById(id);
                customerSummary = new CustomerSummary();
                customerSummary.Accounts.AddRange(cust.Accounts);
                customerSummary.CurrentAddress = cust.Addresses.FirstOrDefault(a => a.IsActive);
            }

            return View(customerSummary);
        }
    }
}
