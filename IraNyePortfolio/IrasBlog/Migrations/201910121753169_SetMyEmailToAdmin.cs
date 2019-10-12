namespace IrasBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetMyEmailToAdmin : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT dbo.AspNetUserRoles (UserId, RoleId) VALUES ((SELECT Id FROM dbo.AspNetUsers WHERE Email = 'iranye@gmail.com'), (SELECT Id FROM dbo.AspNetRoles WHERE [Name] = 'Admin'))");
        }
        
        public override void Down()
        {
            Sql(@"DELETE dbo.AspNetUserRoles WHERE UserId = (SELECT Id FROM dbo.AspNetUsers WHERE Email = 'iranye@gmail.com') AND RoleId = (SELECT Id FROM dbo.AspNetRoles WHERE [Name] = 'Admin')");
        }
    }
}
