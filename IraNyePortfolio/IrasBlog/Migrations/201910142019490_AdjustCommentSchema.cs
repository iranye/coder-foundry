namespace IrasBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustCommentSchema : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "AuthorId", "dbo.AspNetUsers");
            DropIndex("dbo.Comments", new[] { "AuthorId" });
            AlterColumn("dbo.Comments", "AuthorId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Comments", "CommentBody", c => c.String(nullable: false, maxLength: 512));
            CreateIndex("dbo.Comments", "AuthorId");
            AddForeignKey("dbo.Comments", "AuthorId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "AuthorId", "dbo.AspNetUsers");
            DropIndex("dbo.Comments", new[] { "AuthorId" });
            AlterColumn("dbo.Comments", "CommentBody", c => c.String(nullable: false));
            AlterColumn("dbo.Comments", "AuthorId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Comments", "AuthorId");
            AddForeignKey("dbo.Comments", "AuthorId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
