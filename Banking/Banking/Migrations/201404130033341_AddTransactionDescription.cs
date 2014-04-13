namespace Banking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransactionDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transaction", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transaction", "Description");
        }
    }
}
