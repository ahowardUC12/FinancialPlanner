using System.Collections.Generic;

namespace FinancialPlannerApplication.Models.ViewModels
{
    public class BudgetIndexViewModel
    {
        public IList<BudgetViewModel> Budgets { get; set; }
        public IList<BudgetItemViewModel> BudgetItems { get; set; }
        public int SelectedBudgetId { get; set; }
        public int SelectedBudgetItemId { get; set; }
    }
}