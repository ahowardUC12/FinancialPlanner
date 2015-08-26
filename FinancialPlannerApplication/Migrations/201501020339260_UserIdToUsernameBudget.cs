using System.Data.Entity.Migrations;

namespace FinancialPlannerApplication.Migrations
{
    public partial class UserIdToUsernameBudget : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Budgets", "Username", c => c.String());
            DropColumn("dbo.Budgets", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Budgets", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.Budgets", "Username");
        }
    }
}
