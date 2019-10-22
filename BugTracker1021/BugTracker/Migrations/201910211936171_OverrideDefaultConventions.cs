namespace BugTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OverrideDefaultConventions : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "Title", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.ProjectStatus", "Status", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.AspNetUsers", "DisplayName", c => c.String(maxLength: 255));
            AlterColumn("dbo.TicketStatus", "Status", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TicketStatus", "Status", c => c.String());
            AlterColumn("dbo.AspNetUsers", "DisplayName", c => c.String());
            AlterColumn("dbo.ProjectStatus", "Status", c => c.String());
            AlterColumn("dbo.Projects", "Title", c => c.String(nullable: false));
        }
    }
}
