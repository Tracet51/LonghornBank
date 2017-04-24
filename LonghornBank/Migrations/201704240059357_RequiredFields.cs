namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Checkings", "AccountNumber", c => c.String());
            AlterColumn("dbo.IRAs", "AccountNumber", c => c.String());
            AlterColumn("dbo.Savings", "AccountNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Savings", "AccountNumber", c => c.String(nullable: false));
            AlterColumn("dbo.IRAs", "AccountNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Checkings", "AccountNumber", c => c.String(nullable: false));
        }
    }
}
