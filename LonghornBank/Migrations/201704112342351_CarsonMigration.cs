namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarsonMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.IRABankingTransactions", "IRA_IRAID", "dbo.IRAs");
            DropForeignKey("dbo.IRABankingTransactions", "BankingTransaction_BankingTransactionID", "dbo.BankingTransactions");
            DropIndex("dbo.IRABankingTransactions", new[] { "IRA_IRAID" });
            DropIndex("dbo.IRABankingTransactions", new[] { "BankingTransaction_BankingTransactionID" });
            CreateTable(
                "dbo.Managers",
                c => new
                    {
                        ManagerID = c.Int(nullable: false, identity: true),
                        FName = c.String(nullable: false),
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
                .PrimaryKey(t => t.ManagerID);
            
            AddColumn("dbo.IRAs", "BankingTransaction_BankingTransactionID", c => c.Int());
            CreateIndex("dbo.IRAs", "BankingTransaction_BankingTransactionID");
            AddForeignKey("dbo.IRAs", "BankingTransaction_BankingTransactionID", "dbo.BankingTransactions", "BankingTransactionID");
            DropTable("dbo.IRABankingTransactions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.IRABankingTransactions",
                c => new
                    {
                        IRA_IRAID = c.Int(nullable: false),
                        BankingTransaction_BankingTransactionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IRA_IRAID, t.BankingTransaction_BankingTransactionID });
            
            DropForeignKey("dbo.IRAs", "BankingTransaction_BankingTransactionID", "dbo.BankingTransactions");
            DropIndex("dbo.IRAs", new[] { "BankingTransaction_BankingTransactionID" });
            DropColumn("dbo.IRAs", "BankingTransaction_BankingTransactionID");
            DropTable("dbo.Managers");
            CreateIndex("dbo.IRABankingTransactions", "BankingTransaction_BankingTransactionID");
            CreateIndex("dbo.IRABankingTransactions", "IRA_IRAID");
            AddForeignKey("dbo.IRABankingTransactions", "BankingTransaction_BankingTransactionID", "dbo.BankingTransactions", "BankingTransactionID", cascadeDelete: true);
            AddForeignKey("dbo.IRABankingTransactions", "IRA_IRAID", "dbo.IRAs", "IRAID", cascadeDelete: true);
        }
    }
}
