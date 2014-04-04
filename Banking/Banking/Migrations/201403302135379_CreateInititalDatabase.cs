namespace Banking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateInititalDatabase : DbMigration
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
                        Customer_CustomerId = c.Int(),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("dbo.Customer", t => t.Customer_CustomerId)
                .Index(t => t.Customer_CustomerId);
            
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
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Created = c.DateTime(nullable: false),
                        Applied = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        LeftAccount_AccountId = c.Int(),
                        RightAccount_AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.TransactionId)
                .ForeignKey("dbo.Account", t => t.LeftAccount_AccountId)
                .ForeignKey("dbo.Account", t => t.RightAccount_AccountId)
                .Index(t => t.LeftAccount_AccountId)
                .Index(t => t.RightAccount_AccountId);
            
            CreateTable(
                "dbo.Investment",
                c => new
                    {
                        InvestmentId = c.Int(nullable: false, identity: true),
                        TermStart = c.DateTime(nullable: false),
                        TermEnd = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                        CompoundingFrequency = c.Int(nullable: false),
                        Account_AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.InvestmentId)
                .ForeignKey("dbo.Account", t => t.Account_AccountId)
                .Index(t => t.Account_AccountId);
            
            CreateTable(
                "dbo.InvestmentInterval",
                c => new
                    {
                        InvestmentIntervalId = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        InterestRate = c.Double(nullable: false),
                        StartingAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Investment_InvestmentId = c.Int(),
                    })
                .PrimaryKey(t => t.InvestmentIntervalId)
                .ForeignKey("dbo.Investment", t => t.Investment_InvestmentId)
                .Index(t => t.Investment_InvestmentId);
            
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
            DropIndex("dbo.InvestmentInterval", new[] { "Investment_InvestmentId" });
            DropIndex("dbo.Investment", new[] { "Account_AccountId" });
            DropIndex("dbo.Transaction", new[] { "RightAccount_AccountId" });
            DropIndex("dbo.Transaction", new[] { "LeftAccount_AccountId" });
            DropIndex("dbo.Address", new[] { "Customer_CustomerId" });
            DropForeignKey("dbo.CustomerAccounts", "AccountId", "dbo.Account");
            DropForeignKey("dbo.CustomerAccounts", "CustomerId", "dbo.Customer");
            DropForeignKey("dbo.InvestmentInterval", "Investment_InvestmentId", "dbo.Investment");
            DropForeignKey("dbo.Investment", "Account_AccountId", "dbo.Account");
            DropForeignKey("dbo.Transaction", "RightAccount_AccountId", "dbo.Account");
            DropForeignKey("dbo.Transaction", "LeftAccount_AccountId", "dbo.Account");
            DropForeignKey("dbo.Address", "Customer_CustomerId", "dbo.Customer");
            DropTable("dbo.CustomerAccounts");
            DropTable("dbo.InvestmentInterval");
            DropTable("dbo.Investment");
            DropTable("dbo.Transaction");
            DropTable("dbo.Account");
            DropTable("dbo.Address");
            DropTable("dbo.Customer");
        }
    }
}
