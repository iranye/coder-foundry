namespace FinancialPortal.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGetAvatarPathSproc : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "GetAvatarPathByUserID",
                p => new
                {
                    UserID = p.String()
                },
                @"
    SELECT  AvatarPath
    FROM dbo.AspNetUsers
    WHERE Id = @UserID"
            );
        }
        
        public override void Down()
        {
            DropStoredProcedure("GetAvatarPathByUserID");
        }
    }
}
