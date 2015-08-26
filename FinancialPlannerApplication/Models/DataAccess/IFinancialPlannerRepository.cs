using System.Collections.Generic;

namespace FinancialPlannerApplication.Models.DataAccess
{
    public interface IFinancialPlannerRepository
    {
        IEnumerable<Account> GetAccounts();
        IEnumerable<Budget> GetBudgets();
        IEnumerable<BudgetItem> GetBudgetItems();
        IEnumerable<Transaction> GetTransactions();
        IEnumerable<Expense> GetExpenses(); 
        void AddAccount(Account account);
        void EditAccount(Account account);
        void AddTransaction(Transaction transaction);
        void EditTransaction(Transaction transaction);
        void AddExpense(Expense expense);
        void EditExpense(Expense expense);

        void AddBudget(Budget budget);
        void AddBudgetItem(BudgetItem budgetItem);
        void EditBudgetItem(BudgetItem budgetItem);

        void Save();
    }
}
