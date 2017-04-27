namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Round2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Checkings", "AccountNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Checkings", "AccountNumber", c => c.String(nullable: false));
        }
    }
}
