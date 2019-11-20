namespace FinancialPortal.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHouseholdIdToAppUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "HouseholdId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "HouseholdId");
            AddForeignKey("dbo.AspNetUsers", "HouseholdId", "dbo.Households", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "HouseholdId", "dbo.Households");
            DropIndex("dbo.AspNetUsers", new[] { "HouseholdId" });
            DropColumn("dbo.AspNetUsers", "HouseholdId");
        }
    }
}
