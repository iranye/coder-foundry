namespace IrasBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSlugToBlogModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogPosts", "Slug", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BlogPosts", "Slug");
        }
    }
}
