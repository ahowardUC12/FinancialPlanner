using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinancialPlannerApplication.Models.ViewModels
{
    public class EditBudgetItemViewModel
    {
        public int BudgetItemId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Must select a budget")]
        public int SelectedBudgetId { get; set; }
        public IList<BudgetViewModel> Budgets { get; set; }
    }
}