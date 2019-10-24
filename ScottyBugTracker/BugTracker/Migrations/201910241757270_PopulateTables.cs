namespace BugTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateTables : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO TicketPriorities (Name) VALUES ('High')");
            Sql("INSERT INTO TicketPriorities (Name) VALUES ('Medium')");
            Sql("INSERT INTO TicketPriorities (Name) VALUES ('Low')");

            Sql("INSERT INTO TicketStatus (Name) VALUES ('Open')");
            Sql("INSERT INTO TicketStatus (Name) VALUES ('Assigned')");
            Sql("INSERT INTO TicketStatus (Name) VALUES ('OnHold')");
            Sql("INSERT INTO TicketStatus (Name) VALUES ('InProgress')");
            Sql("INSERT INTO TicketStatus (Name) VALUES ('PendingApproval')");
            Sql("INSERT INTO TicketStatus (Name) VALUES ('Resolved')");

            Sql("INSERT INTO TicketTypes (Name) VALUES ('Defect')");
            Sql("INSERT INTO TicketTypes (Name) VALUES ('Feature Request')");
            Sql("INSERT INTO TicketTypes (Name) VALUES ('Request for Documentation')");
            Sql("INSERT INTO TicketTypes (Name) VALUES ('Deployment')");
        }

        public override void Down()
        {
            Sql("DELETE TicketStatus");
            Sql("DELETE TicketPriorities");
            Sql("DELETE TicketTypes");
        }
    }
}
