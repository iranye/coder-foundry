namespace IrasBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeSlugRemoveRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BlogPosts", "Slug", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BlogPosts", "Slug", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
