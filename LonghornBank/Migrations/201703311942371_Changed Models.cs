namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedModels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BankingTransactions", "Checking_CheckingID", "dbo.Checkings");
            DropForeignKey("dbo.BankingTransactions", "Saving_SavingID", "dbo.Savings");
            DropIndex("dbo.BankingTransactions", new[] { "Checking_CheckingID" });
            DropIndex("dbo.BankingTransactions", new[] { "Saving_SavingID" });
            CreateTable(
                "dbo.IRAs",
                c => new
                    {
                        IRAID = c.Int(nullable: false, identity: true),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Name = c.String(nullable: false),
                        RunningTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaxWithdrawl = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PendingBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BankingTransaction_BankingTransactionID = c.Int(),
                    })
                .PrimaryKey(t => t.IRAID)
                .ForeignKey("dbo.BankingTransactions", t => t.BankingTransaction_BankingTransactionID)
                .Index(t => t.BankingTransaction_BankingTransactionID);
            
            CreateTable(
                "dbo.CheckingBankingTransactions",
                c => new
                    {
                        Checking_CheckingID = c.Int(nullable: false),
                        BankingTransaction_BankingTransactionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Checking_CheckingID, t.BankingTransaction_BankingTransactionID })
                .ForeignKey("dbo.Checkings", t => t.Checking_CheckingID, cascadeDelete: true)
                .ForeignKey("dbo.BankingTransactions", t => t.BankingTransaction_BankingTransactionID, cascadeDelete: true)
                .Index(t => t.Checking_CheckingID)
                .Index(t => t.BankingTransaction_BankingTransactionID);
            
            CreateTable(
                "dbo.SavingBankingTransactions",
                c => new
                    {
                        Saving_SavingID = c.Int(nullable: false),
                        BankingTransaction_BankingTransactionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Saving_SavingID, t.BankingTransaction_BankingTransactionID })
                .ForeignKey("dbo.Savings", t => t.Saving_SavingID, cascadeDelete: true)
                .ForeignKey("dbo.BankingTransactions", t => t.BankingTransaction_BankingTransactionID, cascadeDelete: true)
                .Index(t => t.Saving_SavingID)
                .Index(t => t.BankingTransaction_BankingTransactionID);
            
            AlterColumn("dbo.BankingTransactions", "DisputeMessage", c => c.String());
            DropColumn("dbo.BankingTransactions", "Checking_CheckingID");
            DropColumn("dbo.BankingTransactions", "Saving_SavingID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BankingTransactions", "Saving_SavingID", c => c.Int());
            AddColumn("dbo.BankingTransactions", "Checking_CheckingID", c => c.Int());
            DropForeignKey("dbo.SavingBankingTransactions", "BankingTransaction_BankingTransactionID", "dbo.BankingTransactions");
            DropForeignKey("dbo.SavingBankingTransactions", "Saving_SavingID", "dbo.Savings");
            DropForeignKey("dbo.IRAs", "BankingTransaction_BankingTransactionID", "dbo.BankingTransactions");
            DropForeignKey("dbo.CheckingBankingTransactions", "BankingTransaction_BankingTransactionID", "dbo.BankingTransactions");
            DropForeignKey("dbo.CheckingBankingTransactions", "Checking_CheckingID", "dbo.Checkings");
            DropIndex("dbo.SavingBankingTransactions", new[] { "BankingTransaction_BankingTransactionID" });
            DropIndex("dbo.SavingBankingTransactions", new[] { "Saving_SavingID" });
            DropIndex("dbo.CheckingBankingTransactions", new[] { "BankingTransaction_BankingTransactionID" });
            DropIndex("dbo.CheckingBankingTransactions", new[] { "Checking_CheckingID" });
            DropIndex("dbo.IRAs", new[] { "BankingTransaction_BankingTransactionID" });
            AlterColumn("dbo.BankingTransactions", "DisputeMessage", c => c.String(nullable: false));
            DropTable("dbo.SavingBankingTransactions");
            DropTable("dbo.CheckingBankingTransactions");
            DropTable("dbo.IRAs");
            CreateIndex("dbo.BankingTransactions", "Saving_SavingID");
            CreateIndex("dbo.BankingTransactions", "Checking_CheckingID");
            AddForeignKey("dbo.BankingTransactions", "Saving_SavingID", "dbo.Savings", "SavingID");
            AddForeignKey("dbo.BankingTransactions", "Checking_CheckingID", "dbo.Checkings", "CheckingID");
        }
    }
}
