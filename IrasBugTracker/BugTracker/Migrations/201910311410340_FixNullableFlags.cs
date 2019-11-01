namespace BugTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixNullableFlags : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TicketComments", "CommentBody", c => c.String(nullable: false));
            AlterColumn("dbo.Tickets", "Title", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tickets", "Title", c => c.String());
            AlterColumn("dbo.TicketComments", "CommentBody", c => c.String());
        }
    }
}
