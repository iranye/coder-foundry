namespace FinancialPortal.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateTransactionTypeTable : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.TransactionTypes (Type) VALUES ('Candy')");
            Sql("INSERT INTO dbo.TransactionTypes (Type) VALUES ('Clothes')");
            Sql("INSERT INTO dbo.TransactionTypes (Type) VALUES ('Essentials')");
            Sql("INSERT INTO dbo.TransactionTypes (Type) VALUES ('Food')");
            Sql("INSERT INTO dbo.TransactionTypes (Type) VALUES ('Gas')");
            Sql("INSERT INTO dbo.TransactionTypes (Type) VALUES ('Meal')");
            Sql("INSERT INTO dbo.TransactionTypes (Type) VALUES ('Miscellaneous')");
            Sql("INSERT INTO dbo.TransactionTypes (Type) VALUES ('Entertainment')");
            Sql("INSERT INTO dbo.TransactionTypes (Type) VALUES ('Unknown')");
            Sql("INSERT INTO dbo.TransactionTypes (Type) VALUES ('Bill')");
            Sql("INSERT INTO dbo.TransactionTypes (Type) VALUES ('Deposit')");
            Sql("INSERT INTO dbo.TransactionTypes (Type) VALUES ('Expense')");
        }

        public override void Down()
        {
            Sql("DELETE dbo.TransactionTypes");
        }
    }
}
