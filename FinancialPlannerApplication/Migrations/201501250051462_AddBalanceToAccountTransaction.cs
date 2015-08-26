using System.Data.Entity.Migrations;

namespace FinancialPlannerApplication.Migrations
{
    public partial class AddBalanceToAccountTransaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountTransactions", "Balance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountTransactions", "Balance");
        }
    }
}
