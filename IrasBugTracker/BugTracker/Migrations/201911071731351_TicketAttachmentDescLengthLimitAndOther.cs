namespace BugTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketAttachmentDescLengthLimitAndOther : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketNotifications", "IsRead", c => c.Boolean(nullable: false));
            AlterColumn("dbo.TicketAttachments", "Description", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TicketAttachments", "Description", c => c.String());
            DropColumn("dbo.TicketNotifications", "IsRead");
        }
    }
}
