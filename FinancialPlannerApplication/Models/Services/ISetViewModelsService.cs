using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinancialPlannerApplication.Models.ViewModels;

namespace FinancialPlannerApplication.Models.Services
{
    public interface ISetViewModelsService
    {
        List<AccountViewModel> SetAccountViewModels(IEnumerable<Account> accounts);
        List<ExpenseViewModel> SetExpenseViewModels(IEnumerable<Expense> expenses);
        List<BudgetViewModel> SetBudgetViewModels(IEnumerable<Budget> budgets);
        List<TransactionViewModel> SetTransactionViewModels(IEnumerable<Transaction> transactions);
        List<BudgetItemViewModel> SetBudgetItemViewModels(IEnumerable<BudgetItem> budgetItems);
    }
}
