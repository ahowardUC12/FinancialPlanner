using System.Collections.Generic;

namespace FinancialPlannerApplication.Models.ViewModels
{
    public class ExpenseTransactionsViewModel
    {
        public IEnumerable<TransactionViewModel> Transactions { get; set; }
        public decimal Total { get; set; }
    }
}