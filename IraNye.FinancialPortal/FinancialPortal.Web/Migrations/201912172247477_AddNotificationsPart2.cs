namespace FinancialPortal.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotificationsPart2 : DbMigration
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BankAccounts", t => t.BankAccountId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.RecipientId, cascadeDelete: true)
                .Index(t => t.BankAccountId)
                .Index(t => t.RecipientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "RecipientId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notifications", "BankAccountId", "dbo.BankAccounts");
            DropIndex("dbo.Notifications", new[] { "RecipientId" });
            DropIndex("dbo.Notifications", new[] { "BankAccountId" });
            DropTable("dbo.Notifications");
        }
    }
}
