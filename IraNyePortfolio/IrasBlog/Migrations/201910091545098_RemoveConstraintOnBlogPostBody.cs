namespace IrasBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveConstraintOnBlogPostBody : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BlogPosts", "BlogPostBody", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BlogPosts", "BlogPostBody", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
