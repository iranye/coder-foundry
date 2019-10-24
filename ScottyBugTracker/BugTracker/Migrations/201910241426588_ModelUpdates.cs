namespace BugTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelUpdates : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TicketNotifications", name: "UserNotifiedId", newName: "RecipientId");
            RenameIndex(table: "dbo.TicketNotifications", name: "IX_UserNotifiedId", newName: "IX_RecipientId");
            AddColumn("dbo.TicketNotifications", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.TicketNotifications", "Subject", c => c.String());
            AddColumn("dbo.TicketNotifications", "NotificationBody", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TicketNotifications", "NotificationBody");
            DropColumn("dbo.TicketNotifications", "Subject");
            DropColumn("dbo.TicketNotifications", "Created");
            RenameIndex(table: "dbo.TicketNotifications", name: "IX_RecipientId", newName: "IX_UserNotifiedId");
            RenameColumn(table: "dbo.TicketNotifications", name: "RecipientId", newName: "UserNotifiedId");
        }
    }
}
