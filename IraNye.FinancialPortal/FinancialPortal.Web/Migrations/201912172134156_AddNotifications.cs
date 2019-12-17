namespace FinancialPortal.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotifications : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BankAccountId = c.Int(nullable: false),
                        Subject = c.String(nullable: false, maxLength: 255),
                        Body = c.String(nullable: false, maxLength: 255),
                        RecipientId = c.String(nullable: false, maxLength: 128),
                        IsRead = c.Boolean(nullable: false),
                        Household_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BankAccounts", t => t.BankAccountId, cascadeDelete: true)
                .ForeignKey("dbo.Households", t => t.Household_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.RecipientId, cascadeDelete: true)
                .Index(t => t.BankAccountId)
                .Index(t => t.RecipientId)
                .Index(t => t.Household_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "RecipientId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notifications", "Household_Id", "dbo.Households");
            DropForeignKey("dbo.Notifications", "BankAccountId", "dbo.BankAccounts");
            DropIndex("dbo.Notifications", new[] { "Household_Id" });
            DropIndex("dbo.Notifications", new[] { "RecipientId" });
            DropIndex("dbo.Notifications", new[] { "BankAccountId" });
            DropTable("dbo.Notifications");
        }
    }
}
