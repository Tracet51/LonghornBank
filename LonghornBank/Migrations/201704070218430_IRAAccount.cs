namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IRAAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IRAs", "AccountNumber", c => c.String(nullable: false));
            AddColumn("dbo.IRAs", "Customer_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.IRAs", "Customer_Id");
            AddForeignKey("dbo.IRAs", "Customer_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IRAs", "Customer_Id", "dbo.AspNetUsers");
            DropIndex("dbo.IRAs", new[] { "Customer_Id" });
            DropColumn("dbo.IRAs", "Customer_Id");
            DropColumn("dbo.IRAs", "AccountNumber");
        }
    }
}
