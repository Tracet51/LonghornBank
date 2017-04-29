namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inital : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Payees",
                c => new
                    {
                        PayeeID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        StreetAddress = c.String(nullable: false),
                        City = c.String(nullable: false),
                        State = c.String(nullable: false),
                        Zip = c.String(),
                        PhoneNumber = c.String(nullable: false),
                        PayeeType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PayeeID);
            
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
            
            CreateTable(
                "dbo.PayeeAppUsers",
                c => new
                    {
                        Payee_PayeeID = c.Int(nullable: false),
                        AppUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Payee_PayeeID, t.AppUser_Id })
                .ForeignKey("dbo.Payees", t => t.Payee_PayeeID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.AppUser_Id, cascadeDelete: true)
                .Index(t => t.Payee_PayeeID)
                .Index(t => t.AppUser_Id);
            
            AddColumn("dbo.BankingTransactions", "ApprovalStatus", c => c.Int(nullable: false));
            AddColumn("dbo.BankingTransactions", "ManagerDisputeMessage", c => c.String());
            AddColumn("dbo.IRAs", "Fee", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.IRAs", "AccountNumber", c => c.String(nullable: false));
            AlterColumn("dbo.IRAs", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PayeeAppUsers", "AppUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PayeeAppUsers", "Payee_PayeeID", "dbo.Payees");
            DropIndex("dbo.PayeeAppUsers", new[] { "AppUser_Id" });
            DropIndex("dbo.PayeeAppUsers", new[] { "Payee_PayeeID" });
            AlterColumn("dbo.IRAs", "Name", c => c.String());
            AlterColumn("dbo.IRAs", "AccountNumber", c => c.String());
            DropColumn("dbo.IRAs", "Fee");
            DropColumn("dbo.BankingTransactions", "ManagerDisputeMessage");
            DropColumn("dbo.BankingTransactions", "ApprovalStatus");
            DropTable("dbo.PayeeAppUsers");
            DropTable("dbo.Employees");
            DropTable("dbo.Payees");
        }
    }
}
