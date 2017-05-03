namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class overdrawn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Checkings", "Overdrawn", c => c.Boolean(nullable: false));
            AddColumn("dbo.IRAs", "Overdrawn", c => c.Boolean(nullable: false));
            AddColumn("dbo.Savings", "Overdrawn", c => c.Boolean(nullable: false));
            AddColumn("dbo.StockAccounts", "Overdrawn", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StockAccounts", "Overdrawn");
            DropColumn("dbo.Savings", "Overdrawn");
            DropColumn("dbo.IRAs", "Overdrawn");
            DropColumn("dbo.Checkings", "Overdrawn");
        }
    }
}
