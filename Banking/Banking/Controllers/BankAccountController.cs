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

        protected override void Dispose(bool disposing)
        {
            accountRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}