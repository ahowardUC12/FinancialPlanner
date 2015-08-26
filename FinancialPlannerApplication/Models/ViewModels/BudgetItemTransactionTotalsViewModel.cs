using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPlannerApplication.Models.ViewModels
{
    public class BudgetItemTransactionTotalsViewModel
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string BudgetName { get; set; }
    }
}