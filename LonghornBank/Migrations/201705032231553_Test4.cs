namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.IRAs", "BankingTransaction_BankingTransactionID", "dbo.BankingTransactions");
            DropIndex("dbo.IRAs", new[] { "BankingTransaction_BankingTransactionID" });
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
                "dbo.StockAccounts",
                c => new
                    {
                        StockAccountID = c.Int(nullable: false, identity: true),
                        ApprovalStatus = c.Int(nullable: false),
                        CashBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Name = c.String(),
                        AccountNumber = c.String(),
                        PendingBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TradingFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Bounses = c.Boolean(nullable: false),
                        Overdrawn = c.Boolean(nullable: false),
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
            AddColumn("dbo.BankingTransactions", "StockAccount_StockAccountID", c => c.Int());
            AddColumn("dbo.BankingTransactions", "Trade_TradeID", c => c.Int());
            AddColumn("dbo.Checkings", "Overdrawn", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "FiredStatus", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "SSN", c => c.String());
            AddColumn("dbo.IRAs", "Fee", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.IRAs", "Overdrawn", c => c.Boolean(nullable: false));
            AddColumn("dbo.Savings", "Overdrawn", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Checkings", "AccountNumber", c => c.String());
            AlterColumn("dbo.Checkings", "Name", c => c.String());
            AlterColumn("dbo.IRAs", "AccountNumber", c => c.String());
            AlterColumn("dbo.IRAs", "Name", c => c.String());
            AlterColumn("dbo.Savings", "Name", c => c.String());
            AlterColumn("dbo.Savings", "AccountNumber", c => c.String());
            CreateIndex("dbo.BankingTransactions", "StockAccount_StockAccountID");
            CreateIndex("dbo.BankingTransactions", "Trade_TradeID");
            AddForeignKey("dbo.BankingTransactions", "StockAccount_StockAccountID", "dbo.StockAccounts", "StockAccountID");
            AddForeignKey("dbo.BankingTransactions", "Trade_TradeID", "dbo.Trades", "TradeID");
            DropColumn("dbo.IRAs", "BankingTransaction_BankingTransactionID");
            DropTable("dbo.Managers");
        }
        
        public override void Down()
        {
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
            DropForeignKey("dbo.Trades", "StockMarket_StockMarketID", "dbo.StockMarkets");
            DropForeignKey("dbo.Trades", "StockAccount_StockAccountID", "dbo.StockAccounts");
            DropForeignKey("dbo.BankingTransactions", "Trade_TradeID", "dbo.Trades");
            DropForeignKey("dbo.StockAccounts", "Customer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.BankingTransactions", "StockAccount_StockAccountID", "dbo.StockAccounts");
            DropForeignKey("dbo.PayeeAppUsers", "AppUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PayeeAppUsers", "Payee_PayeeID", "dbo.Payees");
            DropForeignKey("dbo.IRABankingTransactions", "BankingTransaction_BankingTransactionID", "dbo.BankingTransactions");
            DropForeignKey("dbo.IRABankingTransactions", "IRA_IRAID", "dbo.IRAs");
            DropIndex("dbo.PayeeAppUsers", new[] { "AppUser_Id" });
            DropIndex("dbo.PayeeAppUsers", new[] { "Payee_PayeeID" });
            DropIndex("dbo.IRABankingTransactions", new[] { "BankingTransaction_BankingTransactionID" });
            DropIndex("dbo.IRABankingTransactions", new[] { "IRA_IRAID" });
            DropIndex("dbo.Trades", new[] { "StockMarket_StockMarketID" });
            DropIndex("dbo.Trades", new[] { "StockAccount_StockAccountID" });
            DropIndex("dbo.StockAccounts", new[] { "Customer_Id" });
            DropIndex("dbo.BankingTransactions", new[] { "Trade_TradeID" });
            DropIndex("dbo.BankingTransactions", new[] { "StockAccount_StockAccountID" });
            AlterColumn("dbo.Savings", "AccountNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Savings", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.IRAs", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.IRAs", "AccountNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Checkings", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Checkings", "AccountNumber", c => c.String(nullable: false));
            DropColumn("dbo.Savings", "Overdrawn");
            DropColumn("dbo.IRAs", "Overdrawn");
            DropColumn("dbo.IRAs", "Fee");
            DropColumn("dbo.AspNetUsers", "SSN");
            DropColumn("dbo.AspNetUsers", "FiredStatus");
            DropColumn("dbo.Checkings", "Overdrawn");
            DropColumn("dbo.BankingTransactions", "Trade_TradeID");
            DropColumn("dbo.BankingTransactions", "StockAccount_StockAccountID");
            DropColumn("dbo.BankingTransactions", "ManagerDisputeMessage");
            DropColumn("dbo.BankingTransactions", "ApprovalStatus");
            DropTable("dbo.PayeeAppUsers");
            DropTable("dbo.IRABankingTransactions");
            DropTable("dbo.StockMarkets");
            DropTable("dbo.Trades");
            DropTable("dbo.StockAccounts");
            DropTable("dbo.Payees");
            CreateIndex("dbo.IRAs", "BankingTransaction_BankingTransactionID");
            AddForeignKey("dbo.IRAs", "BankingTransaction_BankingTransactionID", "dbo.BankingTransactions", "BankingTransactionID");
        }
    }
}
