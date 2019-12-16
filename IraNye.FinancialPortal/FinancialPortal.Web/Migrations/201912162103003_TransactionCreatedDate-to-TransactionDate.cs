namespace FinancialPortal.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionCreatedDatetoTransactionDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "TransactionDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Transactions", "Created");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "Created", c => c.DateTime(nullable: false));
            DropColumn("dbo.Transactions", "TransactionDateTime");
        }
    }
}
