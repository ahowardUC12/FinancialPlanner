namespace FinancialPlannerApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakingDatesRequiredInBudget : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Budgets", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Budgets", "EndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Budgets", "EndDate", c => c.DateTime());
            AlterColumn("dbo.Budgets", "StartDate", c => c.DateTime());
        }
    }
}
