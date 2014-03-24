using System.Web.Mvc;

using Banking.Application.DAL;
using Banking.Application.Web.ViewModels;
using Banking.Domain.Services.BankingOperationsEngine;

namespace Banking.Application.Web.Controllers
{
    using Banking.Application.Core;

    [Authorize(Roles = "Teller")]
    public class TellerController : Controller
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IAccountRepository accountRepository;
        private readonly ICustomerOperationsManager customerOperationsManager;

        public TellerController(
            ICustomerRepository customerRepository,
            IAccountRepository accountRepository,
            ICustomerOperationsManager customerOperationsManager)
        {
            this.customerRepository = customerRepository;
            this.accountRepository = accountRepository;
            this.customerOperationsManager = customerOperationsManager;
        }

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

                if (customer != null)
                {
                    customerSummary = new CustomerSummary();

                    foreach (var account in customer.Accounts)
                    {
                        customerSummary.Accounts.Add(account.ToViewModel());
                    }

                    var address = customerOperationsManager.GetActiveAddress(customer);

                    customerSummary.CurrentAddress = address.ToViewModel();
                }
            }

            return View(customerSummary);
        }

        [HttpGet]
        public ActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCustomer(string firstName, string lastName, string phone, string email)
        {
            var customer = customerOperationsManager.CreateCustomer(firstName, lastName, phone, email);

            customerRepository.AddCustomer(customer);

            customerRepository.Save();

            return RedirectToAction("Home");
        }

        public ActionResult AccountDetails(string accountNumber)
        {
            var account = accountRepository.GetAccountByNumber(accountNumber);

            return View(account.ToViewModel());
        }
    }
}
