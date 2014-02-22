namespace Banking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccountIsLockedTimestamp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Account", "IsLocked", c => c.Boolean(nullable: false));
            AddColumn("dbo.Account", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Account", "Timestamp");
            DropColumn("dbo.Account", "IsLocked");
        }
    }
}
