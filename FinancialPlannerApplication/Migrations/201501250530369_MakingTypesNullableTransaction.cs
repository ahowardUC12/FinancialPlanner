using System.Data.Entity.Migrations;

namespace FinancialPlannerApplication.Migrations
{
    public partial class MakingTypesNullableTransaction : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Transactions", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "BudgetItemId", "dbo.BudgetItems");
            DropIndex("dbo.Transactions", new[] { "BudgetItemId" });
            DropIndex("dbo.Transactions", new[] { "AccountId" });
            AlterColumn("dbo.Transactions", "BudgetItemId", c => c.Int());
            AlterColumn("dbo.Transactions", "AccountId", c => c.Int());
            CreateIndex("dbo.Transactions", "BudgetItemId");
            CreateIndex("dbo.Transactions", "AccountId");
            AddForeignKey("dbo.Transactions", "AccountId", "dbo.Accounts", "Id");
            AddForeignKey("dbo.Transactions", "BudgetItemId", "dbo.BudgetItems", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "BudgetItemId", "dbo.BudgetItems");
            DropForeignKey("dbo.Transactions", "AccountId", "dbo.Accounts");
            DropIndex("dbo.Transactions", new[] { "AccountId" });
            DropIndex("dbo.Transactions", new[] { "BudgetItemId" });
            AlterColumn("dbo.Transactions", "AccountId", c => c.Int(nullable: false));
            AlterColumn("dbo.Transactions", "BudgetItemId", c => c.Int(nullable: false));
            CreateIndex("dbo.Transactions", "AccountId");
            CreateIndex("dbo.Transactions", "BudgetItemId");
            AddForeignKey("dbo.Transactions", "BudgetItemId", "dbo.BudgetItems", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Transactions", "AccountId", "dbo.Accounts", "Id", cascadeDelete: true);
        }
    }
}
