namespace BugTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateStatusesTable : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO TicketStatus (Id, Status) VALUES (1, 'New')");
            Sql("INSERT INTO TicketStatus (Id, Status) VALUES (2, 'Assigned')");
            Sql("INSERT INTO TicketStatus (Id, Status) VALUES (3, 'OnHold')");
            Sql("INSERT INTO TicketStatus (Id, Status) VALUES (4, 'InProgress')");
            Sql("INSERT INTO TicketStatus (Id, Status) VALUES (5, 'PendingApproval')");
            Sql("INSERT INTO TicketStatus (Id, Status) VALUES (6, 'Resolved')");

            Sql("INSERT INTO ProjectStatus (Id, Status) VALUES (1, 'New')");
            Sql("INSERT INTO ProjectStatus (Id, Status) VALUES (2, 'Active')");
            Sql("INSERT INTO ProjectStatus (Id, Status) VALUES (3, 'Design')");
            Sql("INSERT INTO ProjectStatus (Id, Status) VALUES (4, 'Development')");
            Sql("INSERT INTO ProjectStatus (Id, Status) VALUES (5, 'PendingApproval')");
            Sql("INSERT INTO ProjectStatus (Id, Status) VALUES (6, 'OnHold')");
            Sql("INSERT INTO ProjectStatus (Id, Status) VALUES (7, 'Deployed')");

        }

        public override void Down()
        {
            Sql("DELETE TicketStatus WHERE Id IN (1, 2, 3, 4, 5, 6)");
            Sql("DELETE ProjectStatus WHERE Id IN (1, 2, 3, 4, 5, 6, 7)");
        }
    }
}
