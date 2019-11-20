namespace FinancialPortal.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HouseholdId = c.Int(nullable: false),
                        Name = c.String(maxLength: 100),
                        AccountType = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        StartingBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrentBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LowBalanceLevel = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Households", t => t.HouseholdId, cascadeDelete: true)
                .Index(t => t.HouseholdId);
            
            CreateTable(
                "dbo.Households",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Greeting = c.String(maxLength: 255),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BankAccountId = c.Int(nullable: false),
                        BudgetItemId = c.Int(),
                        TransactionTypeId = c.Int(nullable: false),
                        CreatedById = c.String(maxLength: 128),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Created = c.DateTime(nullable: false),
                        Memo = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BankAccounts", t => t.BankAccountId, cascadeDelete: true)
                .ForeignKey("dbo.BudgetItems", t => t.BudgetItemId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.TransactionTypes", t => t.TransactionTypeId, cascadeDelete: true)
                .Index(t => t.BankAccountId)
                .Index(t => t.BudgetItemId)
                .Index(t => t.TransactionTypeId)
                .Index(t => t.CreatedById);
            
            CreateTable(
                "dbo.BudgetItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BudgetId = c.Int(nullable: false),
                        Name = c.String(maxLength: 100),
                        Description = c.String(maxLength: 255),
                        TargetAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrentAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Budgets", t => t.BudgetId, cascadeDelete: true)
                .Index(t => t.BudgetId);
            
            CreateTable(
                "dbo.Budgets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HouseholdId = c.Int(nullable: false),
                        Name = c.String(maxLength: 120),
                        Description = c.String(maxLength: 255),
                        Created = c.DateTime(nullable: false),
                        OwnerId = c.String(maxLength: 128),
                        TargetAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrentAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Households", t => t.HouseholdId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId)
                .Index(t => t.HouseholdId)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.TransactionTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(maxLength: 80),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "AvatarPath", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "TransactionTypeId", "dbo.TransactionTypes");
            DropForeignKey("dbo.Transactions", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Transactions", "BudgetItemId", "dbo.BudgetItems");
            DropForeignKey("dbo.Budgets", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Budgets", "HouseholdId", "dbo.Households");
            DropForeignKey("dbo.BudgetItems", "BudgetId", "dbo.Budgets");
            DropForeignKey("dbo.Transactions", "BankAccountId", "dbo.BankAccounts");
            DropForeignKey("dbo.BankAccounts", "HouseholdId", "dbo.Households");
            DropIndex("dbo.Budgets", new[] { "OwnerId" });
            DropIndex("dbo.Budgets", new[] { "HouseholdId" });
            DropIndex("dbo.BudgetItems", new[] { "BudgetId" });
            DropIndex("dbo.Transactions", new[] { "CreatedById" });
            DropIndex("dbo.Transactions", new[] { "TransactionTypeId" });
            DropIndex("dbo.Transactions", new[] { "BudgetItemId" });
            DropIndex("dbo.Transactions", new[] { "BankAccountId" });
            DropIndex("dbo.BankAccounts", new[] { "HouseholdId" });
            DropColumn("dbo.AspNetUsers", "AvatarPath");
            DropTable("dbo.TransactionTypes");
            DropTable("dbo.Budgets");
            DropTable("dbo.BudgetItems");
            DropTable("dbo.Transactions");
            DropTable("dbo.Households");
            DropTable("dbo.BankAccounts");
        }
    }
}
