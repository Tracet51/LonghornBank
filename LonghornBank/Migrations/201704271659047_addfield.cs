namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BankingTransactions", "ManagerDisputeMessage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BankingTransactions", "ManagerDisputeMessage");
        }
    }
}
