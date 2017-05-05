namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PayeeTests : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Checkings", "AccountDisplay", c => c.String());
            AddColumn("dbo.IRAs", "AccountDisplay", c => c.String());
            AddColumn("dbo.Savings", "AccountDisplay", c => c.String());
            AddColumn("dbo.StockAccounts", "AccountDisplay", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StockAccounts", "AccountDisplay");
            DropColumn("dbo.Savings", "AccountDisplay");
            DropColumn("dbo.IRAs", "AccountDisplay");
            DropColumn("dbo.Checkings", "AccountDisplay");
        }
    }
}
