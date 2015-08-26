using System.Data.Entity.Migrations;

namespace FinancialPlannerApplication.Migrations
{
    public partial class AddBalanceToBudgetItemAndRemoveIsBudgetTransactionFromTransaction : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BudgetTransactions", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.BudgetTransactions", "BudgetItemId", "dbo.BudgetItems");
            DropIndex("dbo.BudgetTransactions", new[] { "BudgetItemId" });
            DropIndex("dbo.BudgetTransactions", new[] { "AccountId" });
            AddColumn("dbo.BudgetItems", "Balance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Transactions", "IsBudgetTransaction");
            DropTable("dbo.BudgetTransactions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.BudgetTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentDate = c.DateTime(nullable: false),
                        BudgetItemId = c.Int(nullable: false),
                        AccountId = c.Int(),
                        IsWithdrawal = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Transactions", "IsBudgetTransaction", c => c.Boolean(nullable: false));
            DropColumn("dbo.BudgetItems", "Balance");
            CreateIndex("dbo.BudgetTransactions", "AccountId");
            CreateIndex("dbo.BudgetTransactions", "BudgetItemId");
            AddForeignKey("dbo.BudgetTransactions", "BudgetItemId", "dbo.BudgetItems", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BudgetTransactions", "AccountId", "dbo.Accounts", "Id");
        }
    }
}
