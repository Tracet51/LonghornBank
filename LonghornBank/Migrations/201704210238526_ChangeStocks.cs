namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeStocks : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.StockAccounts", "StockBalance");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StockAccounts", "StockBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
