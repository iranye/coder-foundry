namespace BugTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectAndTicketTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectStatus",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        OwnerId = c.String(maxLength: 128),
                        AssigneeId = c.String(maxLength: 128),
                        StatusId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 255),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(),
                        TicketStatus_Id = c.Byte(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AssigneeId)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.TicketStatus", t => t.TicketStatus_Id)
                .Index(t => t.ProjectId)
                .Index(t => t.OwnerId)
                .Index(t => t.AssigneeId)
                .Index(t => t.TicketStatus_Id);
            
            CreateTable(
                "dbo.TicketStatus",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "TicketStatus_Id", "dbo.TicketStatus");
            DropForeignKey("dbo.Tickets", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Tickets", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tickets", "AssigneeId", "dbo.AspNetUsers");
            DropIndex("dbo.Tickets", new[] { "TicketStatus_Id" });
            DropIndex("dbo.Tickets", new[] { "AssigneeId" });
            DropIndex("dbo.Tickets", new[] { "OwnerId" });
            DropIndex("dbo.Tickets", new[] { "ProjectId" });
            DropTable("dbo.TicketStatus");
            DropTable("dbo.Tickets");
            DropTable("dbo.ProjectStatus");
            DropTable("dbo.Projects");
        }
    }
}
