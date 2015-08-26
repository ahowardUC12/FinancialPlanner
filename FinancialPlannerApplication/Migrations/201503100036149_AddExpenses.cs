using System.Data.Entity.Migrations;

namespace FinancialPlannerApplication.Migrations
{
    public partial class AddExpenses : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Expenses", "Account_Id", "dbo.Accounts");
            DropIndex("dbo.Expenses", new[] { "Account_Id" });
            AddColumn("dbo.Expenses", "InterestRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Transactions", "ExpenseId", c => c.Int());
            CreateIndex("dbo.Transactions", "ExpenseId");
            AddForeignKey("dbo.Transactions", "ExpenseId", "dbo.Expenses", "Id");
            DropColumn("dbo.Expenses", "FinanceId");
            DropColumn("dbo.Expenses", "Account_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Expenses", "Account_Id", c => c.Int());
            AddColumn("dbo.Expenses", "FinanceId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Transactions", "ExpenseId", "dbo.Expenses");
            DropIndex("dbo.Transactions", new[] { "ExpenseId" });
            DropColumn("dbo.Transactions", "ExpenseId");
            DropColumn("dbo.Expenses", "InterestRate");
            CreateIndex("dbo.Expenses", "Account_Id");
            AddForeignKey("dbo.Expenses", "Account_Id", "dbo.Accounts", "Id");
        }
    }
}
