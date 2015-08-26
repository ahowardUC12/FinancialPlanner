using System.Data.Entity.Migrations;

namespace FinancialPlannerApplication.Migrations
{
    public partial class UserIdToUserNameAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "UserName", c => c.String());
            DropColumn("dbo.Accounts", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Accounts", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.Accounts", "UserName");
        }
    }
}
