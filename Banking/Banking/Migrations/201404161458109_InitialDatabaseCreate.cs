namespace Banking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabaseCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        Line1 = c.String(),
                        Line2 = c.String(),
                        City = c.String(),
                        Province = c.String(),
                        PostalCode = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("dbo.Customer", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        AccountId = c.Int(nullable: false, identity: true),
                        AccountNumber = c.String(),
                        Type = c.Int(nullable: false),
                        Category = c.Int(nullable: false),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsLocked = c.Boolean(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.AccountId);
            
            CreateTable(
                "dbo.Transaction",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        LeftAccountId = c.Int(nullable: false),
                        RightAccountId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Created = c.DateTime(nullable: false),
                        Applied = c.DateTime(),
                        Status = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.TransactionId)
                .ForeignKey("dbo.Account", t => t.LeftAccountId)
                .ForeignKey("dbo.Account", t => t.RightAccountId)
                .Index(t => t.LeftAccountId)
                .Index(t => t.RightAccountId);
            
            CreateTable(
                "dbo.Investment",
                c => new
                    {
                        InvestmentId = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(nullable: false),
                        TermStart = c.DateTime(nullable: false),
                        TermEnd = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                        CompoundingFrequency = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InvestmentId)
                .ForeignKey("dbo.Account", t => t.AccountId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.InvestmentInterval",
                c => new
                    {
                        InvestmentIntervalId = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        InterestRate = c.Double(nullable: false),
                        StartingAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InvestmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InvestmentIntervalId)
                .ForeignKey("dbo.Investment", t => t.InvestmentId)
                .Index(t => t.InvestmentId);
            
            CreateTable(
                "dbo.Log",
                c => new
                    {
                        LogId = c.Long(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        UserName = c.String(),
                        ErrorMessage = c.String(),
                        AdditionalData = c.String(),
                    })
                .PrimaryKey(t => t.LogId);
            
            CreateTable(
                "dbo.CustomerAccounts",
                c => new
                    {
                        CustomerId = c.Int(nullable: false),
                        AccountId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CustomerId, t.AccountId })
                .ForeignKey("dbo.Customer", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Account", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.AccountId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CustomerAccounts", new[] { "AccountId" });
            DropIndex("dbo.CustomerAccounts", new[] { "CustomerId" });
            DropIndex("dbo.InvestmentInterval", new[] { "InvestmentId" });
            DropIndex("dbo.Investment", new[] { "AccountId" });
            DropIndex("dbo.Transaction", new[] { "RightAccountId" });
            DropIndex("dbo.Transaction", new[] { "LeftAccountId" });
            DropIndex("dbo.Address", new[] { "CustomerId" });
            DropForeignKey("dbo.CustomerAccounts", "AccountId", "dbo.Account");
            DropForeignKey("dbo.CustomerAccounts", "CustomerId", "dbo.Customer");
            DropForeignKey("dbo.InvestmentInterval", "InvestmentId", "dbo.Investment");
            DropForeignKey("dbo.Investment", "AccountId", "dbo.Account");
            DropForeignKey("dbo.Transaction", "RightAccountId", "dbo.Account");
            DropForeignKey("dbo.Transaction", "LeftAccountId", "dbo.Account");
            DropForeignKey("dbo.Address", "CustomerId", "dbo.Customer");
            DropTable("dbo.CustomerAccounts");
            DropTable("dbo.Log");
            DropTable("dbo.InvestmentInterval");
            DropTable("dbo.Investment");
            DropTable("dbo.Transaction");
            DropTable("dbo.Account");
            DropTable("dbo.Address");
            DropTable("dbo.Customer");
        }
    }
}
