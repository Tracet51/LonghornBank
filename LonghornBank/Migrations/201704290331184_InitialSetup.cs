namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSetup : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.IRAs", "AccountNumber", c => c.String());
            AlterColumn("dbo.IRAs", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.IRAs", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.IRAs", "AccountNumber", c => c.String(nullable: false));
        }
    }
}
