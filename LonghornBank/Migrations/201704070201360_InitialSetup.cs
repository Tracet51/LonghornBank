namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSetup : DbMigration
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
                        CustomerOpinion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CorrectedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BankingTransactionType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BankingTransactionID);
            
            CreateTable(
                "dbo.Checkings",
                c => new
                    {
                        CheckingID = c.Int(nullable: false, identity: true),
                        AccountNumber = c.String(nullable: false),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PendingBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Name = c.String(nullable: false),
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
                "dbo.Savings",
                c => new
                    {
                        SavingID = c.Int(nullable: false, identity: true),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Name = c.String(nullable: false),
                        AccountNumber = c.String(nullable: false),
                        PendingBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Customer_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SavingID)
                .ForeignKey("dbo.AspNetUsers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.IRAs", "BankingTransaction_BankingTransactionID", "dbo.BankingTransactions");
            DropForeignKey("dbo.Savings", "Customer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SavingBankingTransactions", "BankingTransaction_BankingTransactionID", "dbo.BankingTransactions");
            DropForeignKey("dbo.SavingBankingTransactions", "Saving_SavingID", "dbo.Savings");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Checkings", "Customer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CheckingBankingTransactions", "BankingTransaction_BankingTransactionID", "dbo.BankingTransactions");
            DropForeignKey("dbo.CheckingBankingTransactions", "Checking_CheckingID", "dbo.Checkings");
            DropIndex("dbo.SavingBankingTransactions", new[] { "BankingTransaction_BankingTransactionID" });
            DropIndex("dbo.SavingBankingTransactions", new[] { "Saving_SavingID" });
            DropIndex("dbo.CheckingBankingTransactions", new[] { "BankingTransaction_BankingTransactionID" });
            DropIndex("dbo.CheckingBankingTransactions", new[] { "Checking_CheckingID" });
            DropIndex("dbo.IRAs", new[] { "BankingTransaction_BankingTransactionID" });
            DropIndex("dbo.Savings", new[] { "Customer_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Checkings", new[] { "Customer_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.SavingBankingTransactions");
            DropTable("dbo.CheckingBankingTransactions");
            DropTable("dbo.IRAs");
            DropTable("dbo.Savings");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Checkings");
            DropTable("dbo.BankingTransactions");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
        }
    }
}
