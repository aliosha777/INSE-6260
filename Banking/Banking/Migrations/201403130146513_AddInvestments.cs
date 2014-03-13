namespace Banking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInvestments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Investment",
                c => new
                {
                    InvestmentId = c.Int(nullable: false, identity: true),
                    TermStart = c.DateTime(nullable: false),
                    TermEnd = c.DateTime(nullable: false),
                    Type = c.Int(nullable: false),
                    CompoundingFrequency = c.Int(nullable: false),
                    Account_AccountId = c.Int(),
                })
                .PrimaryKey(t => t.InvestmentId)
                .ForeignKey("dbo.Account", t => t.Account_AccountId)
                .Index(t => t.Account_AccountId);

            CreateTable(
                "dbo.InvestmentIntervalModel",
                c => new
                {
                    InvestmentIntervalId = c.Int(nullable: false, identity: true),
                    Start = c.DateTime(nullable: false),
                    End = c.DateTime(nullable: false),
                    InterestRate = c.Double(nullable: false),
                    StartingAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    InvestmentModel_InvestmentId = c.Int(),
                })
                .PrimaryKey(t => t.InvestmentIntervalId)
                .ForeignKey("dbo.Investment", t => t.InvestmentModel_InvestmentId)
                .Index(t => t.InvestmentModel_InvestmentId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.InvestmentIntervalModel");
            DropTable("dbo.Investment");
        }
    }
}
