namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IRAEnabled : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.IRAs", "BankingTransaction_BankingTransactionID", "dbo.BankingTransactions");
            DropIndex("dbo.IRAs", new[] { "BankingTransaction_BankingTransactionID" });
            CreateTable(
                "dbo.IRABankingTransactions",
                c => new
                    {
                        IRA_IRAID = c.Int(nullable: false),
                        BankingTransaction_BankingTransactionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IRA_IRAID, t.BankingTransaction_BankingTransactionID })
                .ForeignKey("dbo.IRAs", t => t.IRA_IRAID, cascadeDelete: true)
                .ForeignKey("dbo.BankingTransactions", t => t.BankingTransaction_BankingTransactionID, cascadeDelete: true)
                .Index(t => t.IRA_IRAID)
                .Index(t => t.BankingTransaction_BankingTransactionID);
            
            DropColumn("dbo.IRAs", "BankingTransaction_BankingTransactionID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IRAs", "BankingTransaction_BankingTransactionID", c => c.Int());
            DropForeignKey("dbo.IRABankingTransactions", "BankingTransaction_BankingTransactionID", "dbo.BankingTransactions");
            DropForeignKey("dbo.IRABankingTransactions", "IRA_IRAID", "dbo.IRAs");
            DropIndex("dbo.IRABankingTransactions", new[] { "BankingTransaction_BankingTransactionID" });
            DropIndex("dbo.IRABankingTransactions", new[] { "IRA_IRAID" });
            DropTable("dbo.IRABankingTransactions");
            CreateIndex("dbo.IRAs", "BankingTransaction_BankingTransactionID");
            AddForeignKey("dbo.IRAs", "BankingTransaction_BankingTransactionID", "dbo.BankingTransactions", "BankingTransactionID");
        }
    }
}
