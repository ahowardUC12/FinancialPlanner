using FinancialPlannerApplication.Models.ViewModels;

namespace FinancialPlannerApplication.Models.Services
{
    public interface IExpenseService
    {
        ExpenseIndexViewModel MapExpenseIndexViewModel(string username);
        ExpenseTransactionsViewModel MapExpenseTransactionsViewModel(int id);
        EditExpenseViewModel MapEditExpenseViewModel(int id);
        EditExpenseTransactionViewModel MapEditTransactionViewModelForEdit(int id, int expenseId, string username);
        EditExpenseTransactionViewModel MapEditTransactionViewModelForAdd(int id, string username);
        void MapEditTransactionViewModel(EditExpenseTransactionViewModel vm, string username);
        void AdjustAccountBalance(Account account, EditExpenseTransactionViewModel vm);
        Expense AddExpense(EditExpenseViewModel vm, string username);
        bool EditExpense(EditExpenseViewModel vm);
        Transaction AddTransaction(Expense expense, EditExpenseTransactionViewModel vm);
        void AdjustTransactionBalances(Account account, EditExpenseTransactionViewModel vm);
        void AdjustBudgetItemBalance(BudgetItem budgetItem, EditExpenseTransactionViewModel vm);
        Transaction EditTransaction(EditExpenseTransactionViewModel vm);
        void AdjustOldBudgetItemBalance(int? oldBudgetItemId);
        void AdjustNewBudgetItemBalance(EditExpenseTransactionViewModel vm);
        void AdjustTransactionBalances(Account account);
        void AdjustAccountAmount(Account account);
        void AdjustExpenseBalance(Expense expense);
        void AdjustExpenseBalance(Expense expense, Transaction transaction);
    }
}
