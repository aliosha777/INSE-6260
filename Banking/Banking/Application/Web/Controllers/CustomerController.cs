using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Banking.Application.Core;
using Banking.Application.DAL;
using Banking.Application.Web.ViewModels;
using Banking.Domain.Services.BankingOperationsEngine;

namespace Banking.Application.Web.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ICustomerOperationsManager customerOperationsManager;

        public CustomerController(
            ICustomerRepository customerRepository,
            ICustomerOperationsManager customerOperationsManager)
        {
            this.customerRepository = customerRepository;
            this.customerOperationsManager = customerOperationsManager;
        }

        public ActionResult Home()
        {
            return this.View();
        }

        public ActionResult CustomerSummary(int id)
        {
            var customer = customerRepository.GetCustomerById(id);

            var customerSummary = new CustomerSummary
            {
                Accounts = customer.Accounts.Select(account => account.ToViewModel()).ToList(),
                FirstName = customer.FirstName,
                LastName = customer.LastName
            };

            return View(customerSummary);
        }

        public ActionResult PersonalDetails(int id)
        {
            var customer = customerRepository.GetCustomerById(id);

            var address = customerOperationsManager.GetActiveAddress(customer);

            var customerPersonalDetails = new CustomerPersonalInformation
            {
                Address = address.ToViewModel()
            };

            return View(customerPersonalDetails);
        }
    }
}
