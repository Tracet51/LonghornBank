namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeID = c.Int(nullable: false, identity: true),
                        FName = c.String(),
                        LName = c.String(nullable: false),
                        StreetAddress = c.String(nullable: false),
                        City = c.String(nullable: false),
                        State = c.String(nullable: false),
                        Zip = c.String(nullable: false),
                        EmailAddress = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        SSN = c.String(nullable: false),
                        ActiveStatus = c.Boolean(nullable: false),
                        FiredStatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeID);
            
            AddColumn("dbo.BankingTransactions", "ApprovalStatus", c => c.Int(nullable: false));
            AddColumn("dbo.BankingTransactions", "ManagerDisputeMessage", c => c.String());
            AlterColumn("dbo.Checkings", "Name", c => c.String());
            AlterColumn("dbo.Savings", "Name", c => c.String());
            AlterColumn("dbo.Savings", "AccountNumber", c => c.String());
            AlterColumn("dbo.StockAccounts", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StockAccounts", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Savings", "AccountNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Savings", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Checkings", "Name", c => c.String(nullable: false));
            DropColumn("dbo.BankingTransactions", "ManagerDisputeMessage");
            DropColumn("dbo.BankingTransactions", "ApprovalStatus");
            DropTable("dbo.Employees");
        }
    }
}
