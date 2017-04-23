namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Stocks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockAccounts", "AccountNumber", c => c.String());
            AlterColumn("dbo.Trades", "Description", c => c.String());
            AlterColumn("dbo.Trades", "DisputeMessage", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Trades", "DisputeMessage", c => c.String(nullable: false));
            AlterColumn("dbo.Trades", "Description", c => c.String(nullable: false));
            DropColumn("dbo.StockAccounts", "AccountNumber");
        }
    }
}
