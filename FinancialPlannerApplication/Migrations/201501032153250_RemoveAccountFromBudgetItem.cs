using System.Data.Entity.Migrations;

namespace FinancialPlannerApplication.Migrations
{
    public partial class RemoveAccountFromBudgetItem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BudgetItems", "AccountId", "dbo.Accounts");
            DropIndex("dbo.BudgetItems", new[] { "AccountId" });
            DropColumn("dbo.BudgetItems", "AccountId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BudgetItems", "AccountId", c => c.Int(nullable: false));
            CreateIndex("dbo.BudgetItems", "AccountId");
            AddForeignKey("dbo.BudgetItems", "AccountId", "dbo.Accounts", "Id", cascadeDelete: true);
        }
    }
}
