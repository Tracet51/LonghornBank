namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transfer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Checkings", "AccountDisplay", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Checkings", "AccountDisplay");
        }
    }
}
