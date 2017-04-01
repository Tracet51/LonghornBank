namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankingTransactions",
                c => new
                    {
                        BankingTransactionID = c.Int(nullable: false, identity: true),
                        TransactionDispute = c.Int(nullable: false),
                        TransactionDate = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(nullable: false),
                        DisputeMessage = c.String(nullable: false),
                        CustomerOpinion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CorrectedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BankingTransactionType = c.Int(nullable: false),
                        Checking_CheckingID = c.Int(),
                        Saving_SavingID = c.Int(),
                    })
                .PrimaryKey(t => t.BankingTransactionID)
                .ForeignKey("dbo.Checkings", t => t.Checking_CheckingID)
                .ForeignKey("dbo.Savings", t => t.Saving_SavingID)
                .Index(t => t.Checking_CheckingID)
                .Index(t => t.Saving_SavingID);
            
            CreateTable(
                "dbo.Checkings",
                c => new
                    {
                        CheckingID = c.Int(nullable: false, identity: true),
                        AccountNumber = c.String(nullable: false),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PendingBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Name = c.String(nullable: false),
                        Customer_CustomerID = c.Int(),
                    })
                .PrimaryKey(t => t.CheckingID)
                .ForeignKey("dbo.Customers", t => t.Customer_CustomerID)
                .Index(t => t.Customer_CustomerID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerID = c.Int(nullable: false, identity: true),
                        FName = c.String(nullable: false),
                        LName = c.String(nullable: false),
                        StreetAddress = c.String(nullable: false),
                        City = c.String(nullable: false),
                        State = c.String(nullable: false),
                        Zip = c.String(nullable: false),
                        EmailAddress = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        DOB = c.DateTime(nullable: false),
                        ActiveStatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerID);
            
            CreateTable(
                "dbo.Savings",
                c => new
                    {
                        SavingID = c.Int(nullable: false, identity: true),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Name = c.String(nullable: false),
                        AccountNumber = c.String(nullable: false),
                        PendingBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Customer_CustomerID = c.Int(),
                    })
                .PrimaryKey(t => t.SavingID)
                .ForeignKey("dbo.Customers", t => t.Customer_CustomerID)
                .Index(t => t.Customer_CustomerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Savings", "Customer_CustomerID", "dbo.Customers");
            DropForeignKey("dbo.BankingTransactions", "Saving_SavingID", "dbo.Savings");
            DropForeignKey("dbo.Checkings", "Customer_CustomerID", "dbo.Customers");
            DropForeignKey("dbo.BankingTransactions", "Checking_CheckingID", "dbo.Checkings");
            DropIndex("dbo.Savings", new[] { "Customer_CustomerID" });
            DropIndex("dbo.Checkings", new[] { "Customer_CustomerID" });
            DropIndex("dbo.BankingTransactions", new[] { "Saving_SavingID" });
            DropIndex("dbo.BankingTransactions", new[] { "Checking_CheckingID" });
            DropTable("dbo.Savings");
            DropTable("dbo.Customers");
            DropTable("dbo.Checkings");
            DropTable("dbo.BankingTransactions");
        }
    }
}
