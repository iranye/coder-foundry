namespace BugTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketCollectionUpdatesAndStrLenLimits : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TicketHistories", "Property", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.TicketHistories", "OldValue", c => c.String(maxLength: 255));
            AlterColumn("dbo.TicketHistories", "NewValue", c => c.String(maxLength: 255));
            AlterColumn("dbo.TicketNotifications", "Subject", c => c.String(maxLength: 255));
            AlterColumn("dbo.TicketNotifications", "NotificationBody", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TicketNotifications", "NotificationBody", c => c.String());
            AlterColumn("dbo.TicketNotifications", "Subject", c => c.String());
            AlterColumn("dbo.TicketHistories", "NewValue", c => c.String());
            AlterColumn("dbo.TicketHistories", "OldValue", c => c.String());
            AlterColumn("dbo.TicketHistories", "Property", c => c.String());
        }
    }
}
