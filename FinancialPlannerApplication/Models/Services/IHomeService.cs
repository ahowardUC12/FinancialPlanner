using System;
using System.Collections.Generic;
using FinancialPlannerApplication.Models.ViewModels;

namespace FinancialPlannerApplication.Models.Services
{
    public interface IHomeService
    {
        HomeIndexViewModel MapHomeIndexViewModel(string username);
        IEnumerable<BudgetItemTotalsViewModel> GetBudgetItemTotals(string username, int budgetId);
        IEnumerable<BudgetItemTransactionTotalsViewModel> GetBudgetTotalsViewModel(string username, int budgetId);
        IEnumerable<TransactionTotalsViewModel> GetAccountProgressViewModels(int accountId, DateTime? fromDate,
            DateTime? toDate);
        IEnumerable<BudgetProgessViewModel> GetBudgetProgress(string username, int budgetId);
    }
}
