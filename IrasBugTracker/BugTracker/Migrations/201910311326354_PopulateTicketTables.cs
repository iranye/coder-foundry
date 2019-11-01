namespace BugTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateTicketTables : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO TicketPriorities (Name) VALUES ('Unknown')");
            Sql("INSERT INTO TicketPriorities (Name) VALUES ('Low')");
            Sql("INSERT INTO TicketPriorities (Name) VALUES ('Medium')");
            Sql("INSERT INTO TicketPriorities (Name) VALUES ('High')");
            Sql("INSERT INTO TicketPriorities (Name) VALUES ('Critical')");

            Sql("INSERT INTO TicketStatus (Name) VALUES ('Open')");
            Sql("INSERT INTO TicketStatus (Name) VALUES ('Assigned')");
            Sql("INSERT INTO TicketStatus (Name) VALUES ('On Hold')");
            Sql("INSERT INTO TicketStatus (Name) VALUES ('In Progress')");
            Sql("INSERT INTO TicketStatus (Name) VALUES ('Needs Remediation')");
            Sql("INSERT INTO TicketStatus (Name) VALUES ('Pending Approval')");
            Sql("INSERT INTO TicketStatus (Name) VALUES ('Resolved')");

            Sql("INSERT INTO TicketTypes (Name) VALUES ('Defect')");
            Sql("INSERT INTO TicketTypes (Name) VALUES ('Feature Request')");
            Sql("INSERT INTO TicketTypes (Name) VALUES ('Request for Documentation')");
            Sql("INSERT INTO TicketTypes (Name) VALUES ('Other')");
        }

        public override void Down()
        {
            Sql("DELETE TicketStatus");
            Sql("DELETE TicketPriorities");
            Sql("DELETE TicketTypes");
        }
    }
}
