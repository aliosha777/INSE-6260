namespace Banking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionalLogdata : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Log", "AdditionalData", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Log", "AdditionalData");
        }
    }
}
