namespace Banking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerAccountsTable : DbMigration
    {
        public override void Up()
        {
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
            DropForeignKey("dbo.CustomerAccounts", "AccountId", "dbo.Account");
            DropForeignKey("dbo.CustomerAccounts", "CustomerId", "dbo.Customer");
            DropTable("dbo.CustomerAccounts");
        }
    }
}
