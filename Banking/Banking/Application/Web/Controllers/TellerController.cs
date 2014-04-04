using System.Web.Mvc;

using Banking.Application.DAL;
using Banking.Application.Web.ViewModels;
using Banking.Domain.Services.BankingOperationsEngine;

namespace Banking.Application.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Security;

    using Banking.Application.Core;
    using Banking.Domain.Entities;

    using WebMatrix.WebData;

    [Authorize(Roles = "Teller")]
    public class TellerController : Controller
    {
        private const string CurrentCustomerId = "CurrentCustomerId";
        private const string NewCustomerId = "NewCustomerId"; 
       
        private readonly ICustomerRepository customerRepository;
        private readonly IAccountRepository accountRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly ICustomerOperationsManager customerOperationsManager;
        private readonly IAccountOperationsManager accountOperationsManager;

        public TellerController(
            ICustomerRepository customerRepository,
            IAccountRepository accountRepository,
            ITransactionRepository transactionRepository,
            ICustomerOperationsManager customerOperationsManager,
            IAccountOperationsManager accountOperationsManager)
        {
            this.customerRepository = customerRepository;
            this.accountRepository = accountRepository;
            this.transactionRepository = transactionRepository;
            this.customerOperationsManager = customerOperationsManager;
            this.accountOperationsManager = accountOperationsManager;
        }

        public ActionResult Home()
        {
            return View();
        }

        public ActionResult CustomerSummary(string customerId)
        {
            int id;

            CustomerSummary customerSummary = null;

            if (int.TryParse(customerId, out id))
            {
                var customer = customerRepository.GetCustomerById(id);

                if (customer != null)
                {
                    // Easier to keep track of the customer we're working with this way
                    Session[CurrentCustomerId] = customerId;

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
        public ActionResult CreateCustomerStep1()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCustomerStep1(UserViewModel userModel)
        {
            // Create a user account for the new customer
            try
            {
                WebSecurity.CreateUserAndAccount(userModel.UserName, userModel.Password);
                Session.Add("NewCreatedUserName", userModel.UserName);
                return RedirectToAction("CreateCustomerStep2", "Teller");
            }
            catch (MembershipCreateUserException e)
            {
                ModelState.AddModelError(string.Empty, e.StatusCode.ToString());
            }

            return View(userModel);
        }
        
        [HttpGet]
        public ActionResult CreateCustomerStep2()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCustomerStep2(CustomerPersonalInformation customerPersonalInformation)
        {
            var userName = (string)Session["NewCreatedUserName"];

            Session.Remove("NewCreatedUserName");

            // Fill the personal details
            var customer = customerOperationsManager
                .CreateCustomer(
                customerPersonalInformation.FirstName,
                customerPersonalInformation.LastName,
                customerPersonalInformation.Phone,
                customerPersonalInformation.Email);

            customer.UserName = userName;

            customerRepository.AddCustomer(customer);

            Session["NewCustomerId"] = customer.CustomerId;

            return RedirectToAction("CreateCustomerStep3", "Teller");
        }

        [HttpGet]
        public ActionResult CreateCustomerStep3()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCustomerStep3(AddressViewModel addressViewModel)
        {
            // Add an aaddress
            var address = new Address
            {
                Line1 = addressViewModel.Line1,
                Line2 = addressViewModel.Line2,
                City = addressViewModel.City,
                PostalCode = addressViewModel.PostalCode,
                Province = addressViewModel.Province
            };

            // Make the first address active by default
            address.IsActive = true;

            var customerId = (int)Session["NewCustomerId"];

            Session.Remove("NewCustomerId");

            var customer = customerRepository.GetCustomerById(customerId);

            customer.Addresses.Add(address);

            customerRepository.UpdateCustomer(customer, true);

            return RedirectToAction("CustomerSummary", "Teller", new { customerId = customer.CustomerId.ToString() });
        }

        public ActionResult CreateBankAccount()
        {
            // TODO: move this into AccountOperationsManager
            var values = from int e in Enum.GetValues(typeof(AccountTypes))
                         where (AccountTypes)e != AccountTypes.GeneralLedgerCash
                         select new { Id = e, Name = Enum.GetName(typeof(AccountTypes), e) };

            var accountTypesList = new SelectList(values, "Id", "Name");

             ViewBag.AccountTypesList = accountTypesList;

            return View();
        }

        [HttpPost]
        public ActionResult CreateBankAccount(string accountType)
        {
            AccountTypes type;

            Enum.TryParse(accountType, out type);

            int customerId;

            int.TryParse((string)Session[CurrentCustomerId], out customerId);

            var customer = customerRepository.GetCustomerById(customerId);
            var account = accountOperationsManager.CreateAccount(type, customer);

            customer.Accounts.Add(account);
            customerRepository.UpdateCustomer(customer, true);

            return RedirectToAction("CustomerSummary", "Teller", new { customerId = customerId.ToString() });
        }

        public ActionResult AccountDetails(string accountNumber)
        {
            var account = accountRepository.GetAccountByNumber(accountNumber);

            var accountTransactions = transactionRepository.GetAccountTransactions(account);

            var accountDetails = new AccountDetailsViewModel { Account = account.ToViewModel() };

            foreach (var transaction in accountTransactions)
            {
                accountDetails.Transactions.Add(transaction.ToViewModel());
            }
            
            return View(accountDetails);
        }
    }
}
