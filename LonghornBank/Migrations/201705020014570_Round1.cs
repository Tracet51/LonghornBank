namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Round1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockAccounts", "ApprovalStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StockAccounts", "ApprovalStatus");
        }
    }
}
