namespace Banking.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Banking.Application.DAL;
    using Banking.Application.Models;
    using Banking.Domain.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<BankDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BankDBContext context)
        {
            if (!context.Accounts.Any(a => a.Type == AccountTypes.GeneralLedgerCash))
            {
                var creationDate = DateTime.Now;

                var generalCashAccount = new BankAccountModel()
                    {
                        AccountNumber = "xxx-xxx-xxxx",
                        Balance = 10000000,
                        Category = AccountCategories.Asset,
                        Created = creationDate,
                        Modified = creationDate,
                        IsActive = true,
                    };

                context.Accounts.Add(generalCashAccount);
            }
        }
    }
}
