using System;
using System.Collections.Generic;

namespace FinancialPlannerApplication.Models.ViewModels
{
    public class BudgetViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual ICollection<BudgetDetailsViewModel> BudgetItems { get; set; } 
    }
}