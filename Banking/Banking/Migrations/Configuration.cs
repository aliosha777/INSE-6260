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
                        AccountNumber = "123-456-000000",
                        Balance = 10000000,
                        Category = AccountCategories.Asset,
                        Type = AccountTypes.GeneralLedgerCash,
                        Created = creationDate,
                        Modified = creationDate,
                        IsActive = true,
                    };

                context.Accounts.Add(generalCashAccount);
            }
        }
    }
}
