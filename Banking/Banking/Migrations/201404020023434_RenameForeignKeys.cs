namespace Banking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameForeignKeys : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Investment", "Account_AccountId", "dbo.Account");
            DropForeignKey("dbo.InvestmentInterval", "Investment_InvestmentId", "dbo.Investment");
            DropIndex("dbo.Investment", new[] { "Account_AccountId" });
            DropIndex("dbo.InvestmentInterval", new[] { "Investment_InvestmentId" });
            RenameColumn(table: "dbo.Investment", name: "Account_AccountId", newName: "AccountId");
            RenameColumn(table: "dbo.InvestmentInterval", name: "Investment_InvestmentId", newName: "InvestmentId");
            AddForeignKey("dbo.Investment", "AccountId", "dbo.Account", "AccountId", cascadeDelete: true);
            AddForeignKey("dbo.InvestmentInterval", "InvestmentId", "dbo.Investment", "InvestmentId", cascadeDelete: true);
            CreateIndex("dbo.Investment", "AccountId");
            CreateIndex("dbo.InvestmentInterval", "InvestmentId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.InvestmentInterval", new[] { "InvestmentId" });
            DropIndex("dbo.Investment", new[] { "AccountId" });
            DropForeignKey("dbo.InvestmentInterval", "InvestmentId", "dbo.Investment");
            DropForeignKey("dbo.Investment", "AccountId", "dbo.Account");
            RenameColumn(table: "dbo.InvestmentInterval", name: "InvestmentId", newName: "Investment_InvestmentId");
            RenameColumn(table: "dbo.Investment", name: "AccountId", newName: "Account_AccountId");
            CreateIndex("dbo.InvestmentInterval", "Investment_InvestmentId");
            CreateIndex("dbo.Investment", "Account_AccountId");
            AddForeignKey("dbo.InvestmentInterval", "Investment_InvestmentId", "dbo.Investment", "InvestmentId");
            AddForeignKey("dbo.Investment", "Account_AccountId", "dbo.Account", "AccountId");
        }
    }
}
