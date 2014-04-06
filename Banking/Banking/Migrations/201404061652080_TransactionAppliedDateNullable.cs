namespace Banking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionAppliedDateNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Transaction", "Applied", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Transaction", "Applied", c => c.DateTime(nullable: false));
        }
    }
}
