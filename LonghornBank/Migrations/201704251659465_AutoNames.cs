namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AutoNames : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.IRAs", "Name", c => c.String());
            AlterColumn("dbo.Savings", "Name", c => c.String());
            AlterColumn("dbo.StockAccounts", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StockAccounts", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Savings", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.IRAs", "Name", c => c.String(nullable: false));
        }
    }
}
