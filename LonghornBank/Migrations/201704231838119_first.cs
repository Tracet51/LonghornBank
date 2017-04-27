namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.IRAs", "BankingTransaction_BankingTransactionID", "dbo.BankingTransactions");
            DropIndex("dbo.IRAs", new[] { "BankingTransaction_BankingTransactionID" });
            CreateTable(
                "dbo.StockAccounts",
                c => new
                    {
                        StockAccountID = c.Int(nullable: false, identity: true),
                        CashBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Name = c.String(nullable: false),
                        TradingFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Bounses = c.Boolean(nullable: false),
                        Customer_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.StockAccountID)
                .ForeignKey("dbo.AspNetUsers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.Trades",
                c => new
                    {
                        TradeID = c.Int(nullable: false, identity: true),
                        TransactionDispute = c.Int(nullable: false),
                        TransactionDate = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                        DisputeMessage = c.String(),
                        CorrectedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TradeType = c.Int(nullable: false),
                        PricePerShare = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        Ticker = c.String(nullable: false),
                        StockAccount_StockAccountID = c.Int(),
                        StockMarket_StockMarketID = c.Int(),
                    })
                .PrimaryKey(t => t.TradeID)
                .ForeignKey("dbo.StockAccounts", t => t.StockAccount_StockAccountID)
                .ForeignKey("dbo.StockMarkets", t => t.StockMarket_StockMarketID)
                .Index(t => t.StockAccount_StockAccountID)
                .Index(t => t.StockMarket_StockMarketID);
            
            CreateTable(
                "dbo.StockMarkets",
                c => new
                    {
                        StockMarketID = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false),
                        Ticker = c.String(nullable: false),
                        StockType = c.Int(nullable: false),
                        Fee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StockPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.StockMarketID);
            
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
            
            AddColumn("dbo.BankingTransactions", "StockAccount_StockAccountID", c => c.Int());
            AddColumn("dbo.BankingTransactions", "Trade_TradeID", c => c.Int());
            AddColumn("dbo.AspNetUsers", "SSN", c => c.String());
            CreateIndex("dbo.BankingTransactions", "StockAccount_StockAccountID");
            CreateIndex("dbo.BankingTransactions", "Trade_TradeID");
            AddForeignKey("dbo.BankingTransactions", "StockAccount_StockAccountID", "dbo.StockAccounts", "StockAccountID");
            AddForeignKey("dbo.BankingTransactions", "Trade_TradeID", "dbo.Trades", "TradeID");
            DropColumn("dbo.IRAs", "BankingTransaction_BankingTransactionID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IRAs", "BankingTransaction_BankingTransactionID", c => c.Int());
            DropForeignKey("dbo.Trades", "StockMarket_StockMarketID", "dbo.StockMarkets");
            DropForeignKey("dbo.Trades", "StockAccount_StockAccountID", "dbo.StockAccounts");
            DropForeignKey("dbo.BankingTransactions", "Trade_TradeID", "dbo.Trades");
            DropForeignKey("dbo.StockAccounts", "Customer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.BankingTransactions", "StockAccount_StockAccountID", "dbo.StockAccounts");
            DropForeignKey("dbo.IRABankingTransactions", "BankingTransaction_BankingTransactionID", "dbo.BankingTransactions");
            DropForeignKey("dbo.IRABankingTransactions", "IRA_IRAID", "dbo.IRAs");
            DropIndex("dbo.IRABankingTransactions", new[] { "BankingTransaction_BankingTransactionID" });
            DropIndex("dbo.IRABankingTransactions", new[] { "IRA_IRAID" });
            DropIndex("dbo.Trades", new[] { "StockMarket_StockMarketID" });
            DropIndex("dbo.Trades", new[] { "StockAccount_StockAccountID" });
            DropIndex("dbo.StockAccounts", new[] { "Customer_Id" });
            DropIndex("dbo.BankingTransactions", new[] { "Trade_TradeID" });
            DropIndex("dbo.BankingTransactions", new[] { "StockAccount_StockAccountID" });
            DropColumn("dbo.AspNetUsers", "SSN");
            DropColumn("dbo.BankingTransactions", "Trade_TradeID");
            DropColumn("dbo.BankingTransactions", "StockAccount_StockAccountID");
            DropTable("dbo.IRABankingTransactions");
            DropTable("dbo.StockMarkets");
            DropTable("dbo.Trades");
            DropTable("dbo.StockAccounts");
            CreateIndex("dbo.IRAs", "BankingTransaction_BankingTransactionID");
            AddForeignKey("dbo.IRAs", "BankingTransaction_BankingTransactionID", "dbo.BankingTransactions", "BankingTransactionID");
        }
    }
}
