namespace FinancialPortal.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotification : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invitations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HouseholdId = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        RecipientEmail = c.String(nullable: false, maxLength: 256),
                        Code = c.Guid(nullable: false),
                        TTL = c.Int(nullable: false),
                        IsValid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Households", t => t.HouseholdId, cascadeDelete: true)
                .Index(t => t.HouseholdId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invitations", "HouseholdId", "dbo.Households");
            DropIndex("dbo.Invitations", new[] { "HouseholdId" });
            DropTable("dbo.Invitations");
        }
    }
}
