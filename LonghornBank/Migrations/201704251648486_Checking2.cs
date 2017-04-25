namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Checking2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Checkings", "Name", c => c.String(nullable: true, defaultValue: "Longhorn Checking"));
        }

        public override void Down()
        {
            AlterColumn("dbo.Checkings", "Name", c => c.String(nullable: false, defaultValue: "Longhorn Checking"));
        }
    }
}
