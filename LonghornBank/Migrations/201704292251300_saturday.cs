namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class saturday : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockAccounts", "PendingBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StockAccounts", "PendingBalance");
        }
    }
}
