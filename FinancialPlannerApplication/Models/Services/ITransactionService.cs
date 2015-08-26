using FinancialPlannerApplication.Models.ViewModels;

namespace FinancialPlannerApplication.Models.Services
{
    public interface ISharedOperationsService
    {
        void AdjustTransactionBalances(int? accountId, Account account);
        void AdjustExpenseBalance(Expense expense);
        void AdjustExpenseBalance(Expense expense, Transaction transaction);
        void AdjustAccountAmount(Account account);
        void EditBudgetItemBalance(BudgetItem budgetItem);
        void AdjustOldBudgetItemBalance(int? oldBudgetItemId);
        void AdjustNewBudgetItemBalance(int? budgetItemId);
        void AdjustBudgetItemBalance(BudgetItem budgetItem, bool isWithdrawal, decimal amount);
        void AdjustAccountBalance(Account account, bool isWithdrawal, decimal amount);
    }
}
