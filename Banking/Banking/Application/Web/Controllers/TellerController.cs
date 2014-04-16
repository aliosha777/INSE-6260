using System.Web.Mvc;

using Banking.Application.DAL;
using Banking.Application.Web.ViewModels;
using Banking.Domain.Services.BankingOperationsEngine;

namespace Banking.Application.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Security;

    using Banking.Application.Core;
    using Banking.Application.Web.Attributes;
    using Banking.Domain.Entities;
    using Banking.Domain.Services.AccountServices;
    using Banking.Domain.Services.AdminOperations;
    using Banking.Exceptions;
    using Banking.Filters;

    using WebMatrix.WebData;

    [Authorize(Roles = "Teller")]
    public class TellerController : Controller
    {
        private const string CurrentCustomerId = "CurrentCustomerId";
       
        private readonly ICustomerRepository customerRepository;
        private readonly IAccountRepository accountRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly ICustomerOperationsManager customerOperationsManager;
        private readonly IAccountOperationsManager accountOperationsManager;
        private readonly IInvestmentManager investmentManager;
        private readonly ICustomerManager customerManager;

        public TellerController(
            ICustomerRepository customerRepository,
            IAccountRepository accountRepository,
            ITransactionRepository transactionRepository,
            ICustomerOperationsManager customerOperationsManager,
            IAccountOperationsManager accountOperationsManager,
            IInvestmentManager investmentManager,
            ICustomerManager customerManager)
        {
            this.customerRepository = customerRepository;
            this.accountRepository = accountRepository;
            this.transactionRepository = transactionRepository;
            this.customerOperationsManager = customerOperationsManager;
            this.accountOperationsManager = accountOperationsManager;
            this.investmentManager = investmentManager;
            this.customerManager = customerManager;
        }

        public new RedirectToRouteResult RedirectToAction(string action, string controller)
        {
            return base.RedirectToAction(action, controller);
        }

        public ActionResult Home()
        {
            var searchTypesList = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = "Search By User Name",
                        Value = "0"
                    },
                    new SelectListItem
                    {
                        Text = "Search By First Name",
                        Value = "1"
                    },
                    new SelectListItem
                    {
                        Text = "Search By Account Number",
                        Value = "2"
                    }
                };

            ViewBag.SearchType = searchTypesList;

            return View();
        }

        public ActionResult SearchResults(string searchType, string searchField)
        {
            var customers = new List<ICustomer>();

            switch (searchType)
            {
                case "0":
                {
                    customers.Add(customerManager.FindCustomerByUsername(searchField));
                    break;
                }
                case "1":
                {
                    customers.AddRange(customerManager.FindCustomerByFirstName(searchField));
                    break;
                }
                case "2":
                {
                    customers.AddRange(customerManager.FindCustomerByAccountNumber(searchField));
                    break;
                }
            }

            return this.View(customers);
        }

        [RedirectIfNoCustomerId(CurrentCustomerId)]
        public ActionResult CustomerSummary(string customerId)
        {
            int id;
            CustomerSummary customerSummary = null;
            ICustomer customer = null;

            // If this is coming from a "Back" button then we should have the customer Id in the Session
            if (string.IsNullOrEmpty(customerId))
            {
                customer = this.GetCurrentCustomer();
            }
            else
            {
                // Entry point to a customer, customer Id provided in url. Keep it in the session 
                if (int.TryParse(customerId, out id))
                {
                    customer = customerRepository.GetCustomerById(id);
                    Session[CurrentCustomerId] = customerId;
                }
            }
            
            if (customer != null)
            {
                customerSummary = this.CreateCustomerSummaryViewModel(customer);
            }
            else
            {
                return RedirectToAction("Home", "Teller");
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
            if (ModelState.IsValid)
            {
                // Create a user account for the new customer
                try
                {
                    WebSecurity.CreateUserAndAccount(userModel.UserName, userModel.Password);

                    // If we reached here then the user was created.
                    var customer = customerOperationsManager.CreateCustomer(userModel.UserName);
                    customerRepository.AddCustomer(customer);
                    Session[CurrentCustomerId] = customer.CustomerId.ToString();

                    return RedirectToAction("CreateCustomerStep2", "Teller");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError(string.Empty, e.StatusCode.ToString());
                }
            }
            return View(userModel);
        }

        [HttpGet]
        public ActionResult CreateCustomerStep2()
        {
            return View();
        }

        [HttpPost]
        [RedirectIfNoCustomerId(CurrentCustomerId)]
        public ActionResult CreateCustomerStep2(CustomerPersonalInformation customerPersonalInformation)
        {
            if (ModelState.IsValid)
            {
                var customer = this.GetCurrentCustomer();

                customer.FirstName = customerPersonalInformation.FirstName;
                customer.LastName = customerPersonalInformation.LastName;
                customer.Email = customerPersonalInformation.Email;
                customer.Phone = customerPersonalInformation.Phone;

                customerRepository.UpdateCustomer(customer, true);

                return RedirectToAction("CreateCustomerStep3", "Teller");
            }

            return View();
        }

        [HttpGet]
        [RedirectIfNoCustomerId(CurrentCustomerId)]
        public ActionResult CreateCustomerStep3()
        {
            return View();
        }

        [HttpPost]
        [RedirectIfNoCustomerId(CurrentCustomerId)]
        public ActionResult CreateCustomerStep3(AddressViewModel addressViewModel)
        {
            if (ModelState.IsValid)
            {
                var customer = GetCurrentCustomer();

                var address = new Address
                {
                    Line1 = addressViewModel.Line1,
                    Line2 = addressViewModel.Line2,
                    City = addressViewModel.City,
                    PostalCode = addressViewModel.PostalCode,
                    Province = addressViewModel.Province,
                    CustomerId = customer.CustomerId,
                    IsActive = true
                };

                customer.Addresses.Add(address);
                customerRepository.UpdateCustomer(customer, true);

                return RedirectToAction("CustomerSummary", "Teller");
            }

            return this.View();
        }

        public ActionResult CreateBankAccount()
        {
             ViewBag.AccountTypesList = 
                 ViewHelpers.GetAccountTypesSelectList(
                    new List<AccountTypes>() { AccountTypes.GeneralLedgerCash} );

            return View();
        }

        [HttpPost]
        [RedirectIfNoCustomerId(CurrentCustomerId)]
        public ActionResult CreateBankAccount(string accountType)
        {
            AccountTypes type;

            Enum.TryParse(accountType, out type);

            var customer = this.GetCurrentCustomer();
            accountOperationsManager.CreateAccount(type, customer);

            return RedirectToAction("CustomerSummary", "Teller");
        }

        [RedirectIfNoCustomerId(CurrentCustomerId)]
        public ActionResult Deposit()
        {
            var customer = this.GetCurrentCustomer();
            var values = ViewHelpers.GetAccountsSelectList(customer.Accounts);

            var viewModel = new AccountsOperationsViewModel
                {
                    Amount = 0,
                    OperationType = OperationTypes.Deposit,
                    AccountsSelectList = new List<SelectListItem>(values)
                };

            return View(viewModel);
        }

        [HttpPost]
        [RedirectIfNoCustomerId(CurrentCustomerId)]
        public ActionResult Deposit(
            [Bind(Include = "Amount, SelectedTargetAccountId")]
            AccountsOperationsViewModel accountsOperations)
        {
            var customer = this.GetCurrentCustomer();
            accountsOperations.OperationType = OperationTypes.Deposit;
            accountsOperations.AccountsSelectList = ViewHelpers.GetAccountsSelectList(customer.Accounts);

            if (ModelState.IsValid)
            {
                if (accountsOperations.Amount < 0)
                {
                    ModelState.AddModelError(string.Empty, "Amount must be positive");
                    return this.View(accountsOperations);
                }
                accountOperationsManager.Deposit(
                customer, accountsOperations.SelectedTargetAccountId, accountsOperations.Amount);
                return RedirectToAction("CustomerSummary", "Teller");
            }

            return this.View(accountsOperations);
        }

        [RedirectIfNoCustomerId(CurrentCustomerId)]
        public ActionResult Withdraw()
        {
            var customer = this.GetCurrentCustomer();

            var viewModel = new AccountsOperationsViewModel
            {
                Amount = 0,
                OperationType = OperationTypes.Withdrawal,
                AccountsSelectList = ViewHelpers.GetAccountsSelectList(customer.Accounts)
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [RedirectIfNoCustomerId(CurrentCustomerId)]
        public ActionResult Withdraw(
            [Bind(Include = "Amount, SelectedSourceAccountId")]
            AccountsOperationsViewModel accountsOperations)
        {
            var customer = this.GetCurrentCustomer();
            accountsOperations.OperationType = OperationTypes.Withdrawal;
            accountsOperations.AccountsSelectList = ViewHelpers.GetAccountsSelectList(customer.Accounts);

            if (ModelState.IsValid)
            {
                if (accountsOperations.Amount < 0)
                {
                    ModelState.AddModelError(string.Empty, "Amount must be positive");
                    return this.View(accountsOperations);
                }

                var success = accountOperationsManager.Withdraw(
                    customer, accountsOperations.SelectedSourceAccountId, accountsOperations.Amount);

                if (success)
                {
                    return RedirectToAction("CustomerSummary", "Teller");
                }

                ModelState.AddModelError(string.Empty, "Insufficient funds in sorce account");
            }
            return this.View(accountsOperations);
        }

        [RedirectIfNoCustomerId(CurrentCustomerId)]
        public ActionResult Transfer()
        {
            var customer = this.GetCurrentCustomer();

            var viewModel = new AccountsOperationsViewModel
            {
                Amount = 0,
                OperationType = OperationTypes.Transfer,
                AccountsSelectList = ViewHelpers.GetAccountsSelectList(customer.Accounts)
            };

            return View(viewModel);
        }

        [HttpPost]
        [RedirectIfNoCustomerId(CurrentCustomerId)]
        public ActionResult Transfer(
            [Bind(Include = "Amount, SelectedSourceAccountId, SelectedTargetAccountId")]
            AccountsOperationsViewModel accountsOperations)
        {
            var customer = this.GetCurrentCustomer();
            accountsOperations.OperationType = OperationTypes.Transfer;
            accountsOperations.AccountsSelectList = ViewHelpers.GetAccountsSelectList(customer.Accounts);

            if (ModelState.IsValid)
            {
                if (accountsOperations.SelectedSourceAccountId == accountsOperations.SelectedTargetAccountId)
                {
                    ModelState.AddModelError(string.Empty, "Source and target accounts cannot be the same");
                    return this.View(accountsOperations);
                }

                if (accountsOperations.Amount < 0)
                {
                    ModelState.AddModelError(string.Empty, "Amount must be positive");
                    return this.View(accountsOperations);
                }

                var success = accountOperationsManager.Transfer(
                    customer,
                    accountsOperations.SelectedSourceAccountId,
                    accountsOperations.SelectedTargetAccountId,
                    accountsOperations.Amount);

                if (success)
                {
                    return RedirectToAction("CustomerSummary", "Teller");
                }

                ModelState.AddModelError(string.Empty, "Insufficient funds in sorce account");
            }

            return this.View(accountsOperations);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "CalculateInterest")]
        [RedirectIfNoCustomerId(CurrentCustomerId)]
        public ActionResult CalculateInterest(
            [Bind(Include = "Type, Frequency, TermDuration, Start, StartingAmount, AccountId")]
            InvestmentViewModel investmentViewModel)
        {
            if (ModelState.IsValid)
            {
                var customer = this.GetCurrentCustomer();

                var gicRate = investmentManager.GetGicInterestrate();

                var interest = investmentManager
                    .CalculateProjectedInterestAtMaturity(
                        investmentViewModel.TermDuration,
                        investmentViewModel.StartingAmount,
                        gicRate);

                investmentViewModel.Interest = interest;
                investmentViewModel.InvesementTypes = ViewHelpers.GetEnumSelectList(typeof(InvestmentTypes));
                investmentViewModel.CompoundingFrequecyList = ViewHelpers.GetEnumSelectList(typeof(CompoundingFrequency));
                investmentViewModel.MaxYears = investmentManager.GetMaxTermInYears();
                investmentViewModel.Rate = gicRate;
                investmentViewModel.AccountsList = ViewHelpers.GetAccountsSelectList(customer.Accounts);
            }
            
            return this.View("Invest", investmentViewModel);
        }

        [RedirectIfNoCustomerId(CurrentCustomerId)]
        public ActionResult Invest()
        {
            var customer = this.GetCurrentCustomer();

            var viewModel = new InvestmentViewModel()
            {
                InvesementTypes = ViewHelpers.GetEnumSelectList(typeof(InvestmentTypes)),
                Start = DateTime.Now,
                MaxYears = investmentManager.GetMaxTermInYears(),
                Rate = investmentManager.GetGicInterestrate(),
                CompoundingFrequecyList = ViewHelpers.GetEnumSelectList(typeof(CompoundingFrequency)),
                AccountsList = 
                    ViewHelpers.GetAccountsSelectList(
                        customer.Accounts.Where(a => a.Type == AccountTypes.Investment))
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Invest")]
        [RedirectIfNoCustomerId(CurrentCustomerId)]
        public ActionResult Invest(InvestmentViewModel investmentViewModel)
        {
            if (ModelState.IsValid)
            {
                var customer = this.GetCurrentCustomer();
                var account = customerOperationsManager.GetAccount(customer, investmentViewModel.AccountId);

                IInvestment investment;

                var success = investmentManager.CreateGicInvestment(
                    investmentViewModel.Start,
                    investmentViewModel.TermDuration,
                    investmentViewModel.StartingAmount,
                    account,
                    out investment);

                if (!success)
                {
                    ModelState.AddModelError(string.Empty, "Cannot invest more that the available balance");

                    investmentViewModel.AccountsList =
                        ViewHelpers.GetAccountsSelectList(
                            customer.Accounts.Where(a => a.Type == AccountTypes.Investment));

                    investmentViewModel.CompoundingFrequecyList = ViewHelpers.GetEnumSelectList(typeof(CompoundingFrequency));
                    return this.View(investmentViewModel);
                }

                return RedirectToAction("InvestmentSummary", "Teller", new { InvestmentId = investment.InvestmentId });
            }

            return this.View();
        }

        public ActionResult InvestmentDetails(int investmentId)
        {
            return this.View();
        }

        [RedirectIfNoCustomerId(CurrentCustomerId)]
        public ActionResult InvestmentSummary()
        {
            var customer = this.GetCurrentCustomer();
            var investments = customerOperationsManager.GetInvestments(customer);

            var investmentViewModelList = 
                investments
                .Select(
                    investment => 
                        new InvestmentViewModel()
                            {
                                Start = investment.TermStart, 
                                End = investment.TermEnd, 
                                Rate = this.investmentManager.GetGicInterestrate(), 
                                Frequency = investment.CompoundingFrequency, 
                                Type = investment.Type,
                                InterestDueDate = investmentManager.GetNextCompoundingDate(investment),
                                InterestAmount = investmentManager.CalculateInterestAtNextCompoundingPoint(investment)
                            }).ToList();

            return this.View(investmentViewModelList);
        }

        [RedirectIfNoCustomerId(CurrentCustomerId)]
        public ActionResult AccountStatement(RequestStatementViewModel requestStatementViewModel)
        {
            int accountId = requestStatementViewModel.AccountId;
            var customer = this.GetCurrentCustomer();
            var account = customerOperationsManager.GetAccount(customer, accountId);

            if (ModelState.IsValid)
            {
                var availableBalance = accountOperationsManager.GetAvailableAccountBalance(account);
                var transactions = transactionRepository.GetAccountTransactions(account);

                var statementBuilder = new AccountStatementBuilder();

                var statement = statementBuilder.BuildAccountStatement(
                    account, 
                    requestStatementViewModel.From,
                    requestStatementViewModel.To,
                    availableBalance,
                    transactions);

                var statementViewModel = ViewHelpers.CreateAccountStatementViewModel(statement);

                return this.View(statementViewModel);
            }

            // Reload the same view with the error 
            var accountDetails = ViewHelpers.CreateAccountDetailsViewModel(account, new List<ITransaction>());
            
            return this.View("AccountDetails", accountDetails);
        }

        [RedirectIfNoCustomerId(CurrentCustomerId)]
        public ActionResult AccountDetails(int accountId)
        {
            var customer = this.GetCurrentCustomer();
            var account = customerOperationsManager.GetAccount(customer, accountId);
            var accountDetails = ViewHelpers.CreateAccountDetailsViewModel(account, new List<ITransaction>());
            
            return View(accountDetails);
        }
        
        [RedirectIfNoCustomerId(CurrentCustomerId)]
        public ActionResult EditPersonalInfo()
        {
            var customer = this.GetCurrentCustomer();
            var viewModel = customer.ToPersonalInformationViewModel();

            return this.View(viewModel);
        }

        [HttpPost]
        [RedirectIfNoCustomerId(CurrentCustomerId)]
        public ActionResult EditPersonalInfo(CustomerPersonalInformation personalInformation)
        {
            if (ModelState.IsValid)
            {
                var customer = this.GetCurrentCustomer();

                customer.FirstName = personalInformation.FirstName;
                customer.LastName = personalInformation.LastName;
                customer.Email = personalInformation.Email;
                customer.Phone = personalInformation.Phone;

                customerRepository.UpdateCustomer(customer, true);

                return RedirectToAction("CustomerSummary", "Teller");
            }

            return this.View(personalInformation);
        }

        [RedirectIfNoCustomerId(CurrentCustomerId)]
        public ActionResult EditAddress()
        {
            var customer = this.GetCurrentCustomer();
            var address = customerOperationsManager.GetActiveAddress(customer);

            if (address == null)
            {
                address = new Address { IsActive = true };

                customer.Addresses.Add(address);
                customerRepository.UpdateCustomer(customer, true);
            }

            return this.View(address.ToViewModel());
        }

        [HttpPost]
        [RedirectIfNoCustomerId(CurrentCustomerId)]
        public ActionResult EditAddress(AddressViewModel address)
        {
            if (ModelState.IsValid)
            {
                var customer = this.GetCurrentCustomer();
                var activeAddress = customerOperationsManager.GetActiveAddress(customer);

                activeAddress.Line1 = address.Line1;
                activeAddress.Line2 = address.Line2;
                activeAddress.City = address.City;
                activeAddress.Province = address.Province;
                activeAddress.PostalCode = address.PostalCode;

                customerRepository.UpdateCustomer(customer, true);

                return RedirectToAction("CustomerSummary", "Teller");
            }

            return this.View(address);
        }

        #region Private Methods

        // TODO: All those private methods should be placed in Application -> Services
        

        private ICustomer GetCurrentCustomer()
        {
            ICustomer customer = null;
            var customerId = (string)Session[CurrentCustomerId];

            if (!string.IsNullOrEmpty(customerId))
            {
                customer = customerRepository.GetCustomerById(int.Parse(customerId));
            }

            return customer;
        }

        private CustomerSummary CreateCustomerSummaryViewModel(ICustomer customer)
        {
            var customerSummary = new CustomerSummary
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName
            };

            foreach (var account in customer.Accounts)
            {
                customerSummary.Accounts.Add(account.ToViewModel());
            }

            var address = customerOperationsManager.GetActiveAddress(customer);

            if (address != null)
            {
                customerSummary.CurrentAddress = address.ToViewModel();
            }

            return customerSummary;
        }

        #endregion
    }
}
