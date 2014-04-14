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
    using Banking.Domain.Services.AdminOperations;
    using Banking.Exceptions;

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
            IEnumerable<ICustomer> customers = null;

            switch (searchType)
            {
                case "0":
                {
                    customers = customerManager.FindCustomerByUsername(searchField);
                    break;
                }
                case "1":
                {
                    customers = customerManager.FindCustomerByFirstName(searchField);
                    break;
                }
                case "2":
                {
                    customers = customerManager.FindCustomerByAccountNumber(searchField);
                    break;
                }
            }

            return this.View(customers);
        }

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
                    Session.Add("NewCreatedUserName", userModel.UserName);
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
        public ActionResult CreateCustomerStep2(CustomerPersonalInformation customerPersonalInformation)
        {
            if (ModelState.IsValid)
            {
                var userName = (string)Session["NewCreatedUserName"];

                Session.Remove("NewCreatedUserName");

                // Fill the personal details
                var customer = customerOperationsManager.CreateCustomer(
                    customerPersonalInformation.FirstName,
                    customerPersonalInformation.LastName,
                    customerPersonalInformation.Phone,
                    customerPersonalInformation.Email);

                customer.UserName = userName;

                customerRepository.AddCustomer(customer, true);

                Session[CurrentCustomerId] = customer.CustomerId;

                return RedirectToAction("CreateCustomerStep3", "Teller");
            }

            return View();
        }

        [HttpGet]
        public ActionResult CreateCustomerStep3()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCustomerStep3(AddressViewModel addressViewModel)
        {
            if (ModelState.IsValid)
            {
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

                var customer = GetCurrentCustomer();

                customer.Addresses.Add(address);
                customerRepository.UpdateCustomer(customer, true);

                return RedirectToAction("CustomerSummary", "Teller");
            }

            return this.View();
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

            var customer = this.GetCurrentCustomer();
            accountOperationsManager.CreateAccount(type, customer);

            return RedirectToAction("CustomerSummary", "Teller");
        }

        public ActionResult Deposit()
        {
            var customer = this.GetCurrentCustomer();
            var values = this.GetAccountsSelectList(customer);

            var viewModel = new AccountsOperationsViewModel
                {
                    Amount = 0,
                    OperationType = OperationTypes.Deposit,
                    AccountsSelectList = new List<SelectListItem>(values)
                };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Deposit(
            [Bind(Include = "Amount, SelectedTargetAccountId")]
            AccountsOperationsViewModel accountsOperations)
        {
            var customer = this.GetCurrentCustomer();

            accountOperationsManager.Deposit(
                customer, accountsOperations.SelectedTargetAccountId, accountsOperations.Amount);

            return RedirectToAction("CustomerSummary", "Teller");
        }

        public ActionResult Withdraw()
        {
            var customer = this.GetCurrentCustomer();

            var viewModel = new AccountsOperationsViewModel
            {
                Amount = 0,
                OperationType = OperationTypes.Withdrawal,
                AccountsSelectList = new List<SelectListItem>(GetAccountsSelectList(customer))
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public ActionResult Withdraw(
            [Bind(Include = "Amount, SelectedSourceAccountId")]
            AccountsOperationsViewModel accountsOperations)
        {
            var customer = this.GetCurrentCustomer();

            var success = accountOperationsManager.Withdraw(
                customer, accountsOperations.SelectedSourceAccountId, accountsOperations.Amount);
            
            if (success)
            {
                return 
                    RedirectToAction("CustomerSummary", "Teller");
            }

            ModelState.AddModelError(string.Empty, "Insufficient funds in sorce account");

            var viewModel = new AccountsOperationsViewModel
            {
                Amount = 0,
                OperationType = OperationTypes.Withdrawal,
                AccountsSelectList = new List<SelectListItem>(GetAccountsSelectList(customer))
            };

            return this.View(viewModel);
        }

        public ActionResult Transfer()
        {
            var customer = this.GetCurrentCustomer();

            var viewModel = new AccountsOperationsViewModel
            {
                Amount = 0,
                OperationType = OperationTypes.Transfer,
                AccountsSelectList = new List<SelectListItem>(GetAccountsSelectList(customer))
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Transfer(
            [Bind(Include = "Amount, SelectedSourceAccountId, SelectedTargetAccountId")]
            AccountsOperationsViewModel accountsOperations)
        {
            var customer = this.GetCurrentCustomer();

            var viewModel = new AccountsOperationsViewModel
            {
                Amount = 0,
                OperationType = OperationTypes.Transfer,
                AccountsSelectList = new List<SelectListItem>(GetAccountsSelectList(customer))
            };

            if (accountsOperations.SelectedSourceAccountId == accountsOperations.SelectedTargetAccountId)
            {
                ModelState.AddModelError(string.Empty, "Source and target accounts cannot be the same");
                return this.View(viewModel);
            }

            var success = accountOperationsManager.Transfer(
                customer,
                accountsOperations.SelectedSourceAccountId,
                accountsOperations.SelectedTargetAccountId,
                accountsOperations.Amount);

            if (success)
            {
                return
                    RedirectToAction("CustomerSummary", "Teller");
            }

            ModelState.AddModelError(string.Empty, "Insufficient funds in sorce account");

            return this.View(viewModel);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "CalculateInterest")]
        public ActionResult CalculateInterest(
            [Bind(Include = "Type, Frequency, TermDuration, Start, StartingAmount, AccountId")]
            InvestmentViewModel investmentViewModel)
        {
            var customer = this.GetCurrentCustomer();

            var gicRate = investmentManager.GetGicInterestrate();

            var interest = investmentManager
                .CalculateProjectedInterestAtMaturity(
                    investmentViewModel.TermDuration,
                    investmentViewModel.StartingAmount,
                    gicRate);

            investmentViewModel.Interest = interest;
            investmentViewModel.InvesementTypes = this.GetEnumSelectList(typeof(InvestmentTypes));
            investmentViewModel.CompoundingFrequecyList = this.GetEnumSelectList(typeof(CompoundingFrequency));
            investmentViewModel.MaxYears = investmentManager.GetMaxTermInYears();
            investmentViewModel.Rate = gicRate;
            investmentViewModel.AccountsList = this.GetAccountsSelectList(customer);

            return this.View("Invest", investmentViewModel);
        }

        public ActionResult Invest()
        {
            var customer = this.GetCurrentCustomer();

            var viewModel = new InvestmentViewModel()
            {
                InvesementTypes = this.GetEnumSelectList(typeof(InvestmentTypes)),
                Start = DateTime.Now,
                MaxYears = investmentManager.GetMaxTermInYears(),
                Rate = investmentManager.GetGicInterestrate(),
                CompoundingFrequecyList = this.GetEnumSelectList(typeof(CompoundingFrequency)),
                AccountsList = this.GetAccountsSelectList(customer)
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Invest")]
        public ActionResult Invest(InvestmentViewModel investmentViewModel)
        {
            if (ModelState.IsValid)
            {
                var customer = this.GetCurrentCustomer();

                var account = customerOperationsManager.GetAccount(customer, investmentViewModel.AccountId);

                var investment = investmentManager.CreateGicInvestment(
                    investmentViewModel.Start, 
                    investmentViewModel.TermDuration, 
                    investmentViewModel.StartingAmount,
                    account);

                return RedirectToAction("InvestmentDetails", "Teller", new { InvestmentId = investment.InvestmentId });
            }

            return this.View();
        }

        public ActionResult InvestmentDetails()
        {
            return this.View();
        }

        public ActionResult InvestmentSummary(int investmentId)
        {
            return this.View();
        }

        public ActionResult AccountStatement(RequestStatementViewModel requestStatementViewModel)
        {
            if (ModelState.IsValid)
            {
                int accountId = requestStatementViewModel.AccountId;
                DateTime from = requestStatementViewModel.From;
                DateTime to = requestStatementViewModel.To;

                var customer = this.GetCurrentCustomer();
                var account = customerOperationsManager.GetAccount(customer, accountId);

                var statement = new AccountStatementViewModel
                {
                    AccountNumber = account.AccountNumber,
                    AccountType = Enum.GetName(typeof(AccountTypes), account.Type),
                    Balance = account.Balance,
                    From = from,
                    To = to
                };

                var statementLines = new List<TransactionViewModel>();
                var transactions = transactionRepository.GetTransactionRange(account, from, to);

                var prevBalance = account.Balance;
                var prevTransValue = 0.0;

                // Could order by date applied as well
                foreach (var transaction in transactions.OrderByDescending(t => t.TransactionId))
                {
                    var transactionViewModel = transaction.ToViewModel();
                    
                    // The accounts in question are liability accounts 
                    // so left means withdrawal and right means deposit
                    if (account.AccountId == transaction.LeftAccount.AccountId)
                    {
                        transactionViewModel.Withdrawal = transaction.Value.ToString();
                        transactionViewModel.AccountBalance = (double)prevBalance + prevTransValue;
                        prevTransValue = (double)transaction.Value;
                    }
                    else
                    {
                        transactionViewModel.Deposit = transaction.Value.ToString();
                        transactionViewModel.AccountBalance = (double)prevBalance + prevTransValue;
                        prevTransValue = -(double)transaction.Value;
                    }

                    prevBalance = (decimal)transactionViewModel.AccountBalance;

                    statementLines.Add(transactionViewModel); 
                }

                statementLines.Reverse();
                statement.Transactions.AddRange(statementLines);
                return this.View(statement);
            }

            // TODO: return to calling page with error message
            return this.View("AccountDetails");
        }

        public ActionResult AccountDetails(int accountId)
        {
            var customer = this.GetCurrentCustomer();
            var account = customerOperationsManager.GetAccount(customer, accountId);

            var accountTransactions = transactionRepository.GetAccountTransactions(account);

            var accountDetails = new AccountDetailsViewModel
            {
                Account = account.ToViewModel()
            };

            foreach (var transaction in accountTransactions)
            {
                accountDetails.Transactions.Add(transaction.ToViewModel());
            }
            
            return View(accountDetails);
        }

        private IEnumerable<SelectListItem> GetAccountsSelectList(ICustomer customer)
        {
            var values =
                customer.Accounts
                .Select(
                    account => new SelectListItem()
                    {
                        Value = account.AccountId.ToString(),
                        Text = FormatAccountDropdownItem(account)
                    });

            return values;
        }

        // TODO: All those private methods should be placed in Application -> Services
        private IEnumerable<SelectListItem> GetEnumSelectList(Type enumType)
        {
            var values = from int e in Enum.GetValues(enumType)
                         select new SelectListItem()
                         {
                             Value = e.ToString(),
                             Text = Enum.GetName(enumType, e)
                         };

            return values.ToList();
        }

        private string FormatAccountDropdownItem(IAccount account)
        {
            return string.Format(
                "{0} {1} {2}",
                account.Type.ToString().PadRight(12, '\xA0'),
                account.AccountNumber,
                account.Balance.ToString("C").PadLeft(10, '\xA0'));
        }

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
            customerSummary.CurrentAddress = address.ToViewModel();

            return customerSummary;
        }
    }
}
