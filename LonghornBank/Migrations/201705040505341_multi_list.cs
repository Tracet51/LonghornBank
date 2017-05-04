namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class multi_list : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IRAs", "AccountDisplay", c => c.String());
            AddColumn("dbo.Savings", "AccountDisplay", c => c.String());
            AddColumn("dbo.StockAccounts", "AccountDisplay", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StockAccounts", "AccountDisplay");
            DropColumn("dbo.Savings", "AccountDisplay");
            DropColumn("dbo.IRAs", "AccountDisplay");
        }
    }
}
