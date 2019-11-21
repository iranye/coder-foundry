namespace FinancialPortal.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGetUsersHouseholdIdSproc : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "GetCurrentUserHouseholdId",
                p => new
                {
                    UserID = p.String()
                },
                @"
                    SELECT h.Id
                    FROM dbo.AspNetUsers u
                    LEFT JOIN dbo.Households h ON h.Id = u.HouseholdId
                    WHERE u.Id = @UserID"
            );
        }
        
        public override void Down()
        {
            DropStoredProcedure("GetCurrentUserHouseholdId");
        }
    }
}
