namespace FinancialPortal.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BankAccountModelUpdates : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BankAccounts", "OwnerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.BankAccounts", "OwnerId");
            AddForeignKey("dbo.BankAccounts", "OwnerId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BankAccounts", "OwnerId", "dbo.AspNetUsers");
            DropIndex("dbo.BankAccounts", new[] { "OwnerId" });
            AlterColumn("dbo.BankAccounts", "OwnerId", c => c.String());
        }
    }
}
