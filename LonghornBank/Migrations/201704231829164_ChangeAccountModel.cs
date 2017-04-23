namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeAccountModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockAccounts", "AccountNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StockAccounts", "AccountNumber");
        }
    }
}
