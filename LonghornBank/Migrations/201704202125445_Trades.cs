namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Trades : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BankingTransactions", "StockAccount_StockAccountID", c => c.Int());
            CreateIndex("dbo.BankingTransactions", "StockAccount_StockAccountID");
            AddForeignKey("dbo.BankingTransactions", "StockAccount_StockAccountID", "dbo.StockAccounts", "StockAccountID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BankingTransactions", "StockAccount_StockAccountID", "dbo.StockAccounts");
            DropIndex("dbo.BankingTransactions", new[] { "StockAccount_StockAccountID" });
            DropColumn("dbo.BankingTransactions", "StockAccount_StockAccountID");
        }
    }
}
