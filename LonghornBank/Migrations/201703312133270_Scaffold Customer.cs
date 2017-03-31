namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScaffoldCustomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IRAs", "Customer_CustomerID", c => c.Int());
            CreateIndex("dbo.IRAs", "Customer_CustomerID");
            AddForeignKey("dbo.IRAs", "Customer_CustomerID", "dbo.Customers", "CustomerID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IRAs", "Customer_CustomerID", "dbo.Customers");
            DropIndex("dbo.IRAs", new[] { "Customer_CustomerID" });
            DropColumn("dbo.IRAs", "Customer_CustomerID");
        }
    }
}
