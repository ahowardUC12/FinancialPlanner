using System.Collections;
using System.Collections.Generic;
using FinancialPlannerApplication.Models.ViewModels;

namespace FinancialPlannerApplication.Models.Services
{
    public interface IBudgetService
    {
        BudgetIndexViewModel MapBudgetIndexViewModel(string username);
        Budget AddBudget(EditBudgetViewModel vm, string username);
        bool EditBudget(EditBudgetViewModel vm);
        EditBudgetItemViewModel MapEditBudgetItemViewModelForAdd(string username);
        BudgetItem AddBudgetItem(EditBudgetItemViewModel vm);
        bool EditBudgetItem(EditBudgetItemViewModel vm);
        EditBudgetItemTransactionViewModel MapEditBudgetItemTransactionViewModelForAdd(string username, int budgetItemId);
        EditBudgetItemTransactionViewModel MapEditBudgetItemTransactionViewModelChangeBudget(string username, EditBudgetItemTransactionViewModel vm);
        Transaction AddBudgetItemTransaction(EditBudgetItemTransactionViewModel vm);
        bool EditBudgetItemForAdd(EditBudgetItemTransactionViewModel vm);
        void EditAccountAmount(EditBudgetItemTransactionViewModel vm, Account account, IList<Transaction> transactions);

        void EditAccountTransactionBalances(EditBudgetItemTransactionViewModel vm, Account account,
            IList<Transaction> transactions);
        BudgetDetailsViewModel MapBudgetDetailsViewModel(int id);
        BudgetItemTransactionViewModel MapBudgetItemTransactionViewModel(int id);
        EditBudgetViewModel MapEditBudgetViewModel(int id);
        EditBudgetItemViewModel MapEditBudgetItemViewModel(int id, string username);
        EditBudgetItemTransactionViewModel MapEditBudgetItemTransactionViewModelForEdit(int id, string username);
        EditBudgetItemTransactionViewModel MapEditBudgetItemTransactionViewModelForChange(EditBudgetItemTransactionViewModel vm, string username);
        void EditBudgetTransaction(Transaction transaction, EditBudgetItemTransactionViewModel vm);
        void EditBudgetItemBalance(BudgetItem budgetItem, Transaction transaction);
        void EditBudgetItemBalance(BudgetItem budgetItem);
        void AdjustExpenseBalance(Expense expense, Transaction transaction);
        void AdjustExpenseBalance(Expense expense);
    }
}
