namespace Banking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccountCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Account", "Category", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Account", "Category");
        }
    }
}
