namespace FinancialPlannerApplication.Models.ViewModels
{
    public class TransactionTotalsViewModel
    {
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Year { get; set; }
    }
}