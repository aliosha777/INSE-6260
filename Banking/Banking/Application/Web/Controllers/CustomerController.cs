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
    using Banking.Application.Web.Attributes;
    using Banking.Domain.Entities;
    using Banking.Domain.Services.AccountServices;

    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly ICustomerOperationsManager customerOperationsManager;
        private readonly IAccountOperationsManager accountOperationsManager;
        private readonly IInvestmentManager investmentManager;

        public CustomerController(
            ICustomerRepository customerRepository,
            ICustomerOperationsManager customerOperationsManager,
            IAccountOperationsManager accountOperationsManager,
            ITransactionRepository transactionRepository,
            IInvestmentManager investmentManager)
        {
            this.customerRepository = customerRepository;
            this.customerOperationsManager = customerOperationsManager;
            this.accountOperationsManager = accountOperationsManager;
            this.transactionRepository = transactionRepository;
            this.investmentManager = investmentManager;
        }

        public ActionResult Home()
        {
            var customer = this.GetCurrentCustomer();

            var customerSummary = new CustomerSummary
            {
                Accounts = customer.Accounts.Select(account => account.ToViewModel()).ToList(),
                FirstName = customer.FirstName,
                LastName = customer.LastName
            };

            var currentAddress = customerOperationsManager.GetActiveAddress(customer);

            if (currentAddress != null)
            {
                customerSummary.CurrentAddress = currentAddress.ToViewModel();
            }

            return this.View(customerSummary);
        }

        public ActionResult EditPersonalInfo()
        {
            var customer = this.GetCurrentCustomer();

            var customerPersonalDetails = customer.ToPersonalInformationViewModel();
            return View(customerPersonalDetails);
        }

        [HttpPost]
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

                return RedirectToAction("Home", "Customer");
            }

            return this.View(personalInformation);
        }

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

                return RedirectToAction("Home", "Customer");
            }

            return this.View(address);
        }

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
                    return RedirectToAction("Home", "Customer");
                }

                ModelState.AddModelError(string.Empty, "Insufficient funds in sorce account");
            }

            return this.View(accountsOperations);
        }

        public ActionResult CreateInvestmentAccount()
        {
            ViewBag.AccountTypesList =
                ViewHelpers.GetAccountTypesSelectList(
                   new List<AccountTypes>()
                       {
                           AccountTypes.GeneralLedgerCash, 
                           AccountTypes.Checking,
                           AccountTypes.Savings
                       });

            return View();
        }

        [HttpPost]
        public ActionResult CreateInvestmentAccount(string accountType)
        {
            AccountTypes type;

            Enum.TryParse(accountType, out type);

            var customer = this.GetCurrentCustomer();
            accountOperationsManager.CreateAccount(type, customer);

            return RedirectToAction("Home", "Customer");
        }

        public ActionResult AccountDetails(int accountId)
        {
            var customer = this.GetCurrentCustomer();
            var account = customerOperationsManager.GetAccount(customer, accountId);
            var accountDetails = ViewHelpers.CreateAccountDetailsViewModel(account, new List<ITransaction>());

            return View(accountDetails);
        }

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
        [MultipleButton(Name = "action", Argument = "CalculateInterest")]
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

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Invest")]
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

                return RedirectToAction("InvestmentSummary", "Customer", new { InvestmentId = investment.InvestmentId });
            }

            return this.View();
        }

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

        #region Private Methods
       
        private ICustomer GetCurrentCustomer()
        {
            var currentUser = HttpContext.User.Identity.Name;

            var customer = customerRepository.GetCustomerByUserName(currentUser);

            return customer;
        } 
        
        #endregion
    }
}
