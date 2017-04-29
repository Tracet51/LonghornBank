namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Round1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.BankingTransactions",
                c => new
                    {
                        BankingTransactionID = c.Int(nullable: false, identity: true),
                        TransactionDispute = c.Int(nullable: false),
                        TransactionDate = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(nullable: false),
                        DisputeMessage = c.String(),
                        ApprovalStatus = c.Int(nullable: false),
                        CustomerOpinion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ManagerDisputeMessage = c.String(),
                        CorrectedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BankingTransactionType = c.Int(nullable: false),
                        StockAccount_StockAccountID = c.Int(),
                        Trade_TradeID = c.Int(),
                    })
                .PrimaryKey(t => t.BankingTransactionID)
                .ForeignKey("dbo.StockAccounts", t => t.StockAccount_StockAccountID)
                .ForeignKey("dbo.Trades", t => t.Trade_TradeID)
                .Index(t => t.StockAccount_StockAccountID)
                .Index(t => t.Trade_TradeID);
            
            CreateTable(
                "dbo.Checkings",
                c => new
                    {
                        CheckingID = c.Int(nullable: false, identity: true),
                        AccountNumber = c.String(),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PendingBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Name = c.String(),
                        Customer_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CheckingID)
                .ForeignKey("dbo.AspNetUsers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FName = c.String(nullable: false),
                        LName = c.String(nullable: false),
                        StreetAddress = c.String(nullable: false),
                        City = c.String(nullable: false),
                        State = c.String(nullable: false),
                        Zip = c.String(nullable: false),
                        DOB = c.DateTime(nullable: false),
                        ActiveStatus = c.Boolean(nullable: false),
                        SSN = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.IRAs",
                c => new
                    {
                        IRAID = c.Int(nullable: false, identity: true),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AccountNumber = c.String(),
                        Name = c.String(),
                        RunningTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaxWithdrawl = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PendingBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Fee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Customer_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.IRAID)
                .ForeignKey("dbo.AspNetUsers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
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
                "dbo.Savings",
                c => new
                    {
                        SavingID = c.Int(nullable: false, identity: true),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Name = c.String(),
                        AccountNumber = c.String(),
                        PendingBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Customer_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SavingID)
                .ForeignKey("dbo.AspNetUsers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.StockAccounts",
                c => new
                    {
                        StockAccountID = c.Int(nullable: false, identity: true),
                        CashBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Name = c.String(),
                        AccountNumber = c.String(),
                        PendingBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Trades", "StockMarket_StockMarketID", "dbo.StockMarkets");
            DropForeignKey("dbo.Trades", "StockAccount_StockAccountID", "dbo.StockAccounts");
            DropForeignKey("dbo.BankingTransactions", "Trade_TradeID", "dbo.Trades");
            DropForeignKey("dbo.StockAccounts", "Customer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.BankingTransactions", "StockAccount_StockAccountID", "dbo.StockAccounts");
            DropForeignKey("dbo.Savings", "Customer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SavingBankingTransactions", "BankingTransaction_BankingTransactionID", "dbo.BankingTransactions");
            DropForeignKey("dbo.SavingBankingTransactions", "Saving_SavingID", "dbo.Savings");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PayeeAppUsers", "AppUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PayeeAppUsers", "Payee_PayeeID", "dbo.Payees");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.IRAs", "Customer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.IRABankingTransactions", "BankingTransaction_BankingTransactionID", "dbo.BankingTransactions");
            DropForeignKey("dbo.IRABankingTransactions", "IRA_IRAID", "dbo.IRAs");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Checkings", "Customer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CheckingBankingTransactions", "BankingTransaction_BankingTransactionID", "dbo.BankingTransactions");
            DropForeignKey("dbo.CheckingBankingTransactions", "Checking_CheckingID", "dbo.Checkings");
            DropIndex("dbo.SavingBankingTransactions", new[] { "BankingTransaction_BankingTransactionID" });
            DropIndex("dbo.SavingBankingTransactions", new[] { "Saving_SavingID" });
            DropIndex("dbo.PayeeAppUsers", new[] { "AppUser_Id" });
            DropIndex("dbo.PayeeAppUsers", new[] { "Payee_PayeeID" });
            DropIndex("dbo.IRABankingTransactions", new[] { "BankingTransaction_BankingTransactionID" });
            DropIndex("dbo.IRABankingTransactions", new[] { "IRA_IRAID" });
            DropIndex("dbo.CheckingBankingTransactions", new[] { "BankingTransaction_BankingTransactionID" });
            DropIndex("dbo.CheckingBankingTransactions", new[] { "Checking_CheckingID" });
            DropIndex("dbo.Trades", new[] { "StockMarket_StockMarketID" });
            DropIndex("dbo.Trades", new[] { "StockAccount_StockAccountID" });
            DropIndex("dbo.StockAccounts", new[] { "Customer_Id" });
            DropIndex("dbo.Savings", new[] { "Customer_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.IRAs", new[] { "Customer_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Checkings", new[] { "Customer_Id" });
            DropIndex("dbo.BankingTransactions", new[] { "Trade_TradeID" });
            DropIndex("dbo.BankingTransactions", new[] { "StockAccount_StockAccountID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.SavingBankingTransactions");
            DropTable("dbo.PayeeAppUsers");
            DropTable("dbo.IRABankingTransactions");
            DropTable("dbo.CheckingBankingTransactions");
            DropTable("dbo.Managers");
            DropTable("dbo.Employees");
            DropTable("dbo.StockMarkets");
            DropTable("dbo.Trades");
            DropTable("dbo.StockAccounts");
            DropTable("dbo.Savings");
            DropTable("dbo.Payees");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.IRAs");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Checkings");
            DropTable("dbo.BankingTransactions");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
        }
    }
}
