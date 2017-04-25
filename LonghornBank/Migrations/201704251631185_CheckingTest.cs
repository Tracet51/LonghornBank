namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CheckingTest : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Checkings", "Name", c => c.String(defaultValue: "Longhorn Checking"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Checkings", "Name", c => c.String(nullable: false, defaultValue: "Longhorn Checking"));
        }
    }
}
