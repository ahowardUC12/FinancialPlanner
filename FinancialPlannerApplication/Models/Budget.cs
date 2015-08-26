using System;
using System.Collections.Generic;

namespace FinancialPlannerApplication.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual ICollection<BudgetItem> BudgetItems { get; set; }
        public string Username { get; set; }
    }
}