using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPlannerApplication.Models.ViewModels
{
    public class BudgetProgessViewModel
    {
        public string Name { get; set; }
        public decimal AmountSpent { get; set; }
        public decimal AmountLeft { get; set; }
        public decimal AmountSpentPerc { get; set; }
        public decimal AmountLeftPerc { get; set; }
    }
}