namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IRAName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.IRAs", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.IRAs", "Name", c => c.String(nullable: false));
        }
    }
}
