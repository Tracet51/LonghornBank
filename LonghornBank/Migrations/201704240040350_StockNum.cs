namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StockNum : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.StockAccounts", "StockValue");
            DropColumn("dbo.StockAccounts", "Gains");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StockAccounts", "Gains", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.StockAccounts", "StockValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
