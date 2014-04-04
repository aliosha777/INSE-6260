namespace Banking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Address", "Customer_CustomerId", "dbo.Customer");
            DropIndex("dbo.Address", new[] { "Customer_CustomerId" });
            RenameColumn(table: "dbo.Address", name: "Customer_CustomerId", newName: "CustomerId");
            AddForeignKey("dbo.Address", "CustomerId", "dbo.Customer", "CustomerId", cascadeDelete: true);
            CreateIndex("dbo.Address", "CustomerId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Address", new[] { "CustomerId" });
            DropForeignKey("dbo.Address", "CustomerId", "dbo.Customer");
            RenameColumn(table: "dbo.Address", name: "CustomerId", newName: "Customer_CustomerId");
            CreateIndex("dbo.Address", "Customer_CustomerId");
            AddForeignKey("dbo.Address", "Customer_CustomerId", "dbo.Customer", "CustomerId");
        }
    }
}
