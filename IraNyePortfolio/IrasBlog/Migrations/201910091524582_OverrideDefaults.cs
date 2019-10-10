namespace IrasBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OverrideDefaults : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "AuthorId", "dbo.AspNetUsers");
            DropIndex("dbo.Comments", new[] { "AuthorId" });
            AlterColumn("dbo.BlogPosts", "Title", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.BlogPosts", "BlogPostBody", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Comments", "AuthorId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Comments", "CommentBody", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false, maxLength: 255));
            CreateIndex("dbo.Comments", "AuthorId");
            AddForeignKey("dbo.Comments", "AuthorId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "AuthorId", "dbo.AspNetUsers");
            DropIndex("dbo.Comments", new[] { "AuthorId" });
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AlterColumn("dbo.Comments", "CommentBody", c => c.String());
            AlterColumn("dbo.Comments", "AuthorId", c => c.String(maxLength: 128));
            AlterColumn("dbo.BlogPosts", "BlogPostBody", c => c.String());
            AlterColumn("dbo.BlogPosts", "Title", c => c.String());
            CreateIndex("dbo.Comments", "AuthorId");
            AddForeignKey("dbo.Comments", "AuthorId", "dbo.AspNetUsers", "Id");
        }
    }
}
