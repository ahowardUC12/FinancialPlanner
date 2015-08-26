using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPlannerApplication.Models.ViewModels
{
    public class EditExpenseTransactionViewModel
    {
        public int ExpenseTransactionId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool IsWithdrawal { get; set; }
        public int? SelectedAccountId { get; set; }
        public IList<AccountViewModel> Accounts { get; set; }
        public int SelectedBudgetId { get; set; }
        public IList<BudgetViewModel> Budgets { get; set; }
        public int? SelectedBudgetItemId { get; set; }
        public IList<BudgetItemViewModel> BudgetItems { get; set; }
        public int SelectedExpenseId { get; set; }
        public IList<ExpenseViewModel> Expenses { get; set; }
        public bool NewTransaction { get; set; }
        public int OldExpenseId { get; set; }
        public bool WasWithdrawal { get; set; }
        public decimal OldAmount { get; set; }
        public bool FindingBudgetItems { get; set; }
    }
}