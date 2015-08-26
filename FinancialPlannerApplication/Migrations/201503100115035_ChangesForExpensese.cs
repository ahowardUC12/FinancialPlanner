using System.Data.Entity.Migrations;

namespace FinancialPlannerApplication.Migrations
{
    public partial class ChangesForExpensese : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expenses", "Balance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Expenses", "Username", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Expenses", "Username");
            DropColumn("dbo.Expenses", "Balance");
        }
    }
}
