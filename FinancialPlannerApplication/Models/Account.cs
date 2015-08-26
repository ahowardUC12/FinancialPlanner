namespace FinancialPlannerApplication.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal InitialAmount { get; set; }
        public string UserName { get; set; }
    }
}