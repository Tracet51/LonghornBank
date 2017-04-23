namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManagerStuff : DbMigration
    {
        public override void Up()
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
            
            AlterColumn("dbo.StockAccounts", "Bounses", c => c.Boolean(nullable: false));
            DropColumn("dbo.StockAccounts", "AccountNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StockAccounts", "AccountNumber", c => c.String());
            AlterColumn("dbo.StockAccounts", "Bounses", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropTable("dbo.Managers");
        }
    }
}
