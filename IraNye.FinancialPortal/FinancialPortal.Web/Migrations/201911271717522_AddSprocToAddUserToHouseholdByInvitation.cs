namespace FinancialPortal.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSprocToAddUserToHouseholdByInvitation : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "AddUserToHouseholdByEmail",
                p => new
                {
                    Email = p.String(),
                    HouseholdId = p.Int()
                },
                @"
    UPDATE dbo.AspNetUsers SET HouseholdId = @HouseholdId
	WHERE Email = @Email

	IF NOT EXISTS 
	(
		SELECT UserId FROM dbo.AspNetUserRoles WHERE UserId = (SELECT Id FROM dbo.AspNetUsers WHERE Email = @Email)
		AND RoleId = (SELECT Id FROM dbo.AspNetRoles WHERE [Name] = 'Member')
	)
	BEGIN
		INSERT dbo.AspNetUserRoles (UserId, RoleId) 
		VALUES ((SELECT Id FROM dbo.AspNetUsers WHERE Email = @Email), (SELECT Id FROM dbo.AspNetRoles WHERE [Name] = 'Member'))
	END

	DELETE dbo.Invitations WHERE HouseholdId = @HouseholdId AND RecipientEmail = @Email"
            );
        }
        
        public override void Down()
        {
            DropStoredProcedure("AddUserToHouseholdByEmail");
        }
    }
}
