namespace FinancialPlannerApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovingVirtualsFromTransaction : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Transactions", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "BudgetItemId", "dbo.BudgetItems");
            DropForeignKey("dbo.Transactions", "ExpenseId", "dbo.Expenses");
            DropIndex("dbo.Transactions", new[] { "BudgetItemId" });
            DropIndex("dbo.Transactions", new[] { "AccountId" });
            DropIndex("dbo.Transactions", new[] { "ExpenseId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Transactions", "ExpenseId");
            CreateIndex("dbo.Transactions", "AccountId");
            CreateIndex("dbo.Transactions", "BudgetItemId");
            AddForeignKey("dbo.Transactions", "ExpenseId", "dbo.Expenses", "Id");
            AddForeignKey("dbo.Transactions", "BudgetItemId", "dbo.BudgetItems", "Id");
            AddForeignKey("dbo.Transactions", "AccountId", "dbo.Accounts", "Id");
        }
    }
}
