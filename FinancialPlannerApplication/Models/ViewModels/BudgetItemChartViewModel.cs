using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace FinancialPlannerApplication.Models.ViewModels
{
    public class BudgetItemChartViewModel
    {
        public int BudgetItemId { get; set; }
        public string BudgetItemName { get; set; }
        public decimal BudgetItemAmount { get; set; }
        public decimal BudgetItemBalance { get; set; }
    }
}