namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StockTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockAccounts", "StockValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.StockAccounts", "Gains", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StockAccounts", "Gains");
            DropColumn("dbo.StockAccounts", "StockValue");
        }
    }
}
