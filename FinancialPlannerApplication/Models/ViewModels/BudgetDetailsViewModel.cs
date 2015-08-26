using System.Collections.Generic;

namespace FinancialPlannerApplication.Models.ViewModels
{
    public class BudgetDetailsViewModel
    {
        public IList<BudgetItemViewModel> BudgetItems { get; set; } 
        public BudgetViewModel Budget { get; set; }
    }
}