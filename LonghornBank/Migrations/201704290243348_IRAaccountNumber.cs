namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IRAaccountNumber : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.IRAs", "AccountNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.IRAs", "AccountNumber", c => c.String(nullable: false));
        }
    }
}
