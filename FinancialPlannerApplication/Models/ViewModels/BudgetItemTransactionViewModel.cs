using System.Collections.Generic;

namespace FinancialPlannerApplication.Models.ViewModels
{
    public class BudgetItemTransactionViewModel
    {
        public IList<TransactionViewModel> BudgetItemTransactions { get; set; }
        public int BudgetId { get; set; }
        public decimal TransactionTotal { get; set; }
        public int BudgetItemId { get; set; }
    }
}