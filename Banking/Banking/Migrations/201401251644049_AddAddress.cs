namespace Banking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAddress : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        AddressId = c.String(nullable: false, maxLength: 128),
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
            
            DropColumn("dbo.Customer", "Address");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customer", "Address", c => c.String());
            DropIndex("dbo.Address", new[] { "Customer_CustomerId" });
            DropForeignKey("dbo.Address", "Customer_CustomerId", "dbo.Customer");
            DropTable("dbo.Address");
        }
    }
}
