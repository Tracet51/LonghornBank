namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FiredStatus", c => c.Boolean(nullable: false));
            AddColumn("dbo.StockAccounts", "ApprovalStatus", c => c.Int(nullable: false));
            AlterColumn("dbo.Employees", "FName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "FName", c => c.String());
            DropColumn("dbo.StockAccounts", "ApprovalStatus");
            DropColumn("dbo.AspNetUsers", "FiredStatus");
        }
    }
}
