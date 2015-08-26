using System.Data.Entity.Migrations;

namespace FinancialPlannerApplication.Migrations
{
    public partial class BudgetTransactionAccountIdNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BudgetTransactions", "AccountId", "dbo.Accounts");
            DropIndex("dbo.BudgetTransactions", new[] { "AccountId" });
            AlterColumn("dbo.BudgetTransactions", "AccountId", c => c.Int());
            CreateIndex("dbo.BudgetTransactions", "AccountId");
            AddForeignKey("dbo.BudgetTransactions", "AccountId", "dbo.Accounts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BudgetTransactions", "AccountId", "dbo.Accounts");
            DropIndex("dbo.BudgetTransactions", new[] { "AccountId" });
            AlterColumn("dbo.BudgetTransactions", "AccountId", c => c.Int(nullable: false));
            CreateIndex("dbo.BudgetTransactions", "AccountId");
            AddForeignKey("dbo.BudgetTransactions", "AccountId", "dbo.Accounts", "Id", cascadeDelete: true);
        }
    }
}
