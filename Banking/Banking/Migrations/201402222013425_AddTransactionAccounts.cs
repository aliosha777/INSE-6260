namespace Banking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransactionAccounts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transaction", "LeftAccount_AccountId", c => c.Int());
            AddColumn("dbo.Transaction", "RightAccount_AccountId", c => c.Int());
            AddForeignKey("dbo.Transaction", "LeftAccount_AccountId", "dbo.Account", "AccountId");
            AddForeignKey("dbo.Transaction", "RightAccount_AccountId", "dbo.Account", "AccountId");
            CreateIndex("dbo.Transaction", "LeftAccount_AccountId");
            CreateIndex("dbo.Transaction", "RightAccount_AccountId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Transaction", new[] { "RightAccount_AccountId" });
            DropIndex("dbo.Transaction", new[] { "LeftAccount_AccountId" });
            DropForeignKey("dbo.Transaction", "RightAccount_AccountId", "dbo.Account");
            DropForeignKey("dbo.Transaction", "LeftAccount_AccountId", "dbo.Account");
            DropColumn("dbo.Transaction", "RightAccount_AccountId");
            DropColumn("dbo.Transaction", "LeftAccount_AccountId");
        }
    }
}
