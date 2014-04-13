using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Banking.Application.Web.Controllers
{
    using Banking.Application.Core;
    using Banking.Application.DAL;
    using Banking.Application.Web.ViewModels;
    using Banking.Domain.Entities;
    using Banking.Domain.Services.BankingOperationsEngine;

    [Authorize(Roles = "Admin")]
    public class SystemController : Controller
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly ITransactionEngine transactionEngine;
        private readonly IInvestmentRepository investmentRepository;

        public SystemController(
            ITransactionRepository transactionRepository,
            ITransactionEngine transactionEngine,
            IInvestmentRepository investmentRepository)
        {
            this.transactionRepository = transactionRepository;
            this.transactionEngine = transactionEngine;
            this.investmentRepository = investmentRepository;
        }

        public ActionResult Index()
        {
            var unprocessedTransactions = transactionRepository.GetPendingTransactions();

            var today = DateTime.Now.Date;

            ViewData["Transactions"] = 
                unprocessedTransactions
                .Select(t => new Tuple<ITransaction, bool>(t, transactionEngine.IsTransactionDue(t)));

            return View();
        }

        [HttpPost]
        public ActionResult ProcessTransactions()
        {
            var pendingTransactions = transactionRepository.GetPendingTransactions();

            foreach (var transaction in pendingTransactions)
            {
                transactionEngine.ApplyTransaction(transaction);
            }

            return RedirectToAction("Index");
        }
    }
}
