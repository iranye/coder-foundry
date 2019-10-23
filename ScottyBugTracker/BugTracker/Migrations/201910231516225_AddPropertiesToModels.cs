namespace BugTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPropertiesToModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(),
                        ProjectId = c.Int(nullable: false),
                        TicketPriorityId = c.Int(nullable: false),
                        TicketStatusId = c.Int(nullable: false),
                        TicketTypeId = c.Int(nullable: false),
                        OwnerId = c.String(maxLength: 128),
                        AssignedToId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AssignedToId)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.TicketPriorities", t => t.TicketPriorityId, cascadeDelete: true)
                .ForeignKey("dbo.TicketStatus", t => t.TicketStatusId, cascadeDelete: true)
                .ForeignKey("dbo.TicketTypes", t => t.TicketTypeId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.TicketPriorityId)
                .Index(t => t.TicketStatusId)
                .Index(t => t.TicketTypeId)
                .Index(t => t.OwnerId)
                .Index(t => t.AssignedToId);
            
            CreateTable(
                "dbo.TicketPriorities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TicketStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TicketTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TicketAttachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MediaPath = c.String(),
                        Description = c.String(),
                        CreatedDateTime = c.DateTime(nullable: false),
                        TicketId = c.Int(nullable: false),
                        CreatedById = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.Tickets", t => t.TicketId, cascadeDelete: true)
                .Index(t => t.TicketId)
                .Index(t => t.CreatedById);
            
            CreateTable(
                "dbo.TicketComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        CommentBody = c.String(),
                        TicketId = c.Int(nullable: false),
                        AuthorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorId)
                .ForeignKey("dbo.Tickets", t => t.TicketId, cascadeDelete: true)
                .Index(t => t.TicketId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.TicketHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Property = c.String(),
                        OldValue = c.String(),
                        NewValue = c.String(),
                        ChangedDateTime = c.DateTime(nullable: false),
                        TicketId = c.Int(nullable: false),
                        ChangedById = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ChangedById)
                .ForeignKey("dbo.Tickets", t => t.TicketId, cascadeDelete: true)
                .Index(t => t.TicketId)
                .Index(t => t.ChangedById);
            
            CreateTable(
                "dbo.TicketNotifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TicketId = c.Int(nullable: false),
                        UserNotifiedId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tickets", t => t.TicketId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserNotifiedId)
                .Index(t => t.TicketId)
                .Index(t => t.UserNotifiedId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketNotifications", "UserNotifiedId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TicketNotifications", "TicketId", "dbo.Tickets");
            DropForeignKey("dbo.TicketHistories", "TicketId", "dbo.Tickets");
            DropForeignKey("dbo.TicketHistories", "ChangedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.TicketComments", "TicketId", "dbo.Tickets");
            DropForeignKey("dbo.TicketComments", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TicketAttachments", "TicketId", "dbo.Tickets");
            DropForeignKey("dbo.TicketAttachments", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tickets", "TicketTypeId", "dbo.TicketTypes");
            DropForeignKey("dbo.Tickets", "TicketStatusId", "dbo.TicketStatus");
            DropForeignKey("dbo.Tickets", "TicketPriorityId", "dbo.TicketPriorities");
            DropForeignKey("dbo.Tickets", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Tickets", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tickets", "AssignedToId", "dbo.AspNetUsers");
            DropIndex("dbo.TicketNotifications", new[] { "UserNotifiedId" });
            DropIndex("dbo.TicketNotifications", new[] { "TicketId" });
            DropIndex("dbo.TicketHistories", new[] { "ChangedById" });
            DropIndex("dbo.TicketHistories", new[] { "TicketId" });
            DropIndex("dbo.TicketComments", new[] { "AuthorId" });
            DropIndex("dbo.TicketComments", new[] { "TicketId" });
            DropIndex("dbo.TicketAttachments", new[] { "CreatedById" });
            DropIndex("dbo.TicketAttachments", new[] { "TicketId" });
            DropIndex("dbo.Tickets", new[] { "AssignedToId" });
            DropIndex("dbo.Tickets", new[] { "OwnerId" });
            DropIndex("dbo.Tickets", new[] { "TicketTypeId" });
            DropIndex("dbo.Tickets", new[] { "TicketStatusId" });
            DropIndex("dbo.Tickets", new[] { "TicketPriorityId" });
            DropIndex("dbo.Tickets", new[] { "ProjectId" });
            DropTable("dbo.TicketNotifications");
            DropTable("dbo.TicketHistories");
            DropTable("dbo.TicketComments");
            DropTable("dbo.TicketAttachments");
            DropTable("dbo.TicketTypes");
            DropTable("dbo.TicketStatus");
            DropTable("dbo.TicketPriorities");
            DropTable("dbo.Tickets");
            DropTable("dbo.Projects");
        }
    }
}
