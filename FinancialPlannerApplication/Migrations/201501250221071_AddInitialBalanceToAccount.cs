using System.Data.Entity.Migrations;

namespace FinancialPlannerApplication.Migrations
{
    public partial class AddInitialBalanceToAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "InitialAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "InitialAmount");
        }
    }
}
