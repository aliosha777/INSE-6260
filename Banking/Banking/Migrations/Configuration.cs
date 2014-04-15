namespace Banking.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Web.Security;

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
            // Seed roles in a ghetto way
            var roles = new string[] { "Admin", "Teller", "Customer" };

            foreach (var role in roles)
            {
                context.Database.ExecuteSqlCommand(
                @"if not exists (select RoleId from webpages_Roles where RoleName like @role)
                   begin 
                   insert into webpages_Roles (RoleName) values (@role)
                   end", 
                       new SqlParameter("role", role));
            }

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
