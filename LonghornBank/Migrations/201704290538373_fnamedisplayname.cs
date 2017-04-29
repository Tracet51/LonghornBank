namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fnamedisplayname : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "FName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "FName", c => c.String());
        }
    }
}
