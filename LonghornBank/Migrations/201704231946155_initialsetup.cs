namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialsetup : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Payees", "Customer_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Payees", new[] { "Customer_Id" });
            CreateTable(
                "dbo.PayeeAppUsers",
                c => new
                    {
                        Payee_PayeeID = c.Int(nullable: false),
                        AppUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Payee_PayeeID, t.AppUser_Id })
                .ForeignKey("dbo.Payees", t => t.Payee_PayeeID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.AppUser_Id, cascadeDelete: true)
                .Index(t => t.Payee_PayeeID)
                .Index(t => t.AppUser_Id);
            
            DropColumn("dbo.Payees", "Customer_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payees", "Customer_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.PayeeAppUsers", "AppUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PayeeAppUsers", "Payee_PayeeID", "dbo.Payees");
            DropIndex("dbo.PayeeAppUsers", new[] { "AppUser_Id" });
            DropIndex("dbo.PayeeAppUsers", new[] { "Payee_PayeeID" });
            DropTable("dbo.PayeeAppUsers");
            CreateIndex("dbo.Payees", "Customer_Id");
            AddForeignKey("dbo.Payees", "Customer_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
