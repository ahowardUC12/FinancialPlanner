using System.Collections.Generic;

namespace FinancialPlannerApplication.Models.ViewModels
{
    public class AccountTransactionsViewModel
    {
        public IList<TransactionViewModel> Transactions { get; set; }
        public decimal TransactionTotal { get; set; }
    }
}