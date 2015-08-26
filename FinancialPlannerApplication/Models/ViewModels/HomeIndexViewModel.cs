using System;
using System.Collections.Generic;

namespace FinancialPlannerApplication.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public IList<BudgetViewModel> Budgets { get; set; }
        public int SelectedBudgetId { get; set; }
        public IList<AccountViewModel> Accounts { get; set; }
        public int SelectedAccountId { get; set; }
        public IList<ExpenseProgessViewModel> ExpenseProgess { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}