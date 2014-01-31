namespace Banking.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Banking.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Banking.DAL.BankDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Banking.DAL.BankDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            ////var customers = new List<Customer>();

            ////for(int i = 1; i < 20; i++)
            ////{
            ////    var cust = new Customer()
            ////    {
            ////        FirstName = "CusrtomerFirst" + i,
            ////        LastName = "CustomerLast" + i,
            ////        Address = "Montreal", 
            ////        Email = "customer" + i +"@acme.com",
            ////        Phone = "555-555-5555",
            ////    };

            ////    var account = new Account();

            ////    cust.Accounts.Add(account);
            ////    customers.Add(cust);
            ////}
        }
    }
}
