using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Banking.Models;
using Banking.DAL;

namespace Banking.Controllers
{
    public class BankAccountController : Controller
    {
        private readonly IAccountRepository accountRepository;

        public BankAccountController(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        // GET: /BankAccount/

        public ActionResult Index()
        {
            return View(accountRepository.GetAllAccounts().ToList());
        }

        // GET: /BankAccount/Details/5

        public ActionResult Details(int id = 0)
        {
            Account account = accountRepository.GetAccountById(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: /BankAccount/Create

        public ActionResult Create()
        {
            return View();
        }

        // POST: /BankAccount/Create

        [HttpPost]
        public ActionResult Create(Account account)
        {
            if (ModelState.IsValid)
            {
                accountRepository.InsertAccount(account);
                accountRepository.Save();
                return RedirectToAction("Index");
            }

            return View(account);
        }

        // GET: /BankAccount/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Account account = accountRepository.GetAccountById(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        protected override void Dispose(bool disposing)
        {
            accountRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}