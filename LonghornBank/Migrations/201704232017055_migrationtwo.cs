namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrationtwo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BankingTransactions", "ApprovalStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BankingTransactions", "ApprovalStatus");
        }
    }
}
