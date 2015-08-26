using System.Data.Entity.Migrations;

namespace FinancialPlannerApplication.Migrations
{
    public partial class ConsolidatingTransactionTypes : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AccountTransactions", newName: "Transactions");
            AddColumn("dbo.Transactions", "BudgetItemId", c => c.Int(nullable: false));
            AddColumn("dbo.Transactions", "IsBudgetTransaction", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Transactions", "BudgetItemId");
            AddForeignKey("dbo.Transactions", "BudgetItemId", "dbo.BudgetItems", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "BudgetItemId", "dbo.BudgetItems");
            DropIndex("dbo.Transactions", new[] { "BudgetItemId" });
            DropColumn("dbo.Transactions", "IsBudgetTransaction");
            DropColumn("dbo.Transactions", "BudgetItemId");
            RenameTable(name: "dbo.Transactions", newName: "AccountTransactions");
        }
    }
}
