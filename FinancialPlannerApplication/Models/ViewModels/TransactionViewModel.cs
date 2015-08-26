using System;

namespace FinancialPlannerApplication.Models.ViewModels
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public int BudgetItemId { get; set; }
        public bool IsWithdrawal { get; set; }
        public decimal Balance { get; set; }
    }
}