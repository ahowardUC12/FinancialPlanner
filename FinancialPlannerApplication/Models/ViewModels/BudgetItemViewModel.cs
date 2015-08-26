namespace FinancialPlannerApplication.Models.ViewModels
{
    public class BudgetItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int BudgetId { get; set; }
        public decimal Balance { get; set; }
    }
}