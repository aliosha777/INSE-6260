using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Banking.Application.Web.Controllers
{
    using Banking.Application.Core;
    using Banking.Application.DAL;
    using Banking.Application.Models;
    using Banking.Application.Web.ViewModels;
    using Banking.Domain.Entities;
    using Banking.Domain.Services.BankingOperationsEngine;

    [Authorize(Roles = "Admin")]
    public class SystemController : Controller
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly ITransactionEngine transactionEngine;
        private readonly IInvestmentRepository investmentRepository;
        private readonly ILoggerRepository loggerRepository;

        public SystemController(
            ITransactionRepository transactionRepository,
            ITransactionEngine transactionEngine,
            IInvestmentRepository investmentRepository,
            ILoggerRepository loggerRepository)
        {
            this.transactionRepository = transactionRepository;
            this.transactionEngine = transactionEngine;
            this.investmentRepository = investmentRepository;
            this.loggerRepository = loggerRepository;
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

        public ActionResult ErrorLog()
        {
            var logs = loggerRepository.GetLogs(20);

            ViewData["Logs"] = logs;

            return this.View();
        }
    }
}
