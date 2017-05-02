namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Round1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FiredStatus", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Employees", "FName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "FName", c => c.String());
            DropColumn("dbo.AspNetUsers", "FiredStatus");
        }
    }
}
