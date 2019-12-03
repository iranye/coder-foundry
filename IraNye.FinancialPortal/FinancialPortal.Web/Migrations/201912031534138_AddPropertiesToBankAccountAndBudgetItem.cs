namespace FinancialPortal.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPropertiesToBankAccountAndBudgetItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BankAccounts", "OwnerId", c => c.String());
            AddColumn("dbo.BudgetItems", "Created", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BudgetItems", "Created");
            DropColumn("dbo.BankAccounts", "OwnerId");
        }
    }
}
