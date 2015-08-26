using System.Collections.Generic;

namespace FinancialPlannerApplication.Models.ViewModels
{
    public class ExpenseIndexViewModel
    {
        public IList<ExpenseViewModel> Expenses { get; set; }
        public int SelectedExpenseId { get; set; }
    }
}