namespace BugTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGetUsersByRoleSproc : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
              "GetUsersInRole",
              p => new
              {
                  Role = p.String()
              },
              @"
    SELECT  u.Id
    FROM dbo.AspNetUsers u
	JOIN dbo.AspNetUserRoles ur ON ur.UserId = u.Id
	JOIN dbo.AspNetRoles r ON r.Id = ur.RoleId
	WHERE r.[Name] = @Role"
            );
        }

        public override void Down()
        {
            DropStoredProcedure("GetUsersInRole");
        }
    }
}
