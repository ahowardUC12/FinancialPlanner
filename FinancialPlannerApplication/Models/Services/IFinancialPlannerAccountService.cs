using System.Collections.Generic;
using FinancialPlannerApplication.Models.ViewModels;

namespace FinancialPlannerApplication.Models.Services
{
    public interface IFinancialPlannerAccountService
    {
        AccountIndexViewModel MapAccountIndexViewModel(string username);
        EditAccountViewModel MapEditAccountViewModel(int id);
        Account AddAccount(string username, EditAccountViewModel vm);
        EditTransactionViewModel MapEditTransactionViewModel(string username);
        bool EditAccount(EditAccountViewModel vm, string username);
        AccountTransactionsViewModel MapAccountTransactionsViewModel(int id);
        EditTransactionViewModel MapEditTransactionViewModelForEdit(int id, int accountId, string username);
        EditTransactionViewModel MapEditTransactionViewModelForAdd(int id, string username);
        void AdjustAccountBalance(Account account, EditTransactionViewModel vm);
        Transaction AddTransaction(Account account, EditTransactionViewModel vm);
        void AdjustBudgetItemBalance(BudgetItem budgetItem, EditTransactionViewModel vm);
        void AdjustTransactionBalances(Account account, EditTransactionViewModel vm);
        void MapEditTransactionViewModel(EditTransactionViewModel vm, string username);
        void EditTransaction(EditTransactionViewModel vm);
        void AdjustOldBudgetItemBalance(int? oldBudgetItemId);
        void AdjustNewBudgetItemBalance(EditTransactionViewModel vm);
        void AdjustTransactionBalances(Account account);
        void AdjustAccountAmount(Account account);
        void AdjustExpenseBalance(Expense expense, Transaction transaction);
        void AdjustExpenseBalance(Expense expense);
    }
}
