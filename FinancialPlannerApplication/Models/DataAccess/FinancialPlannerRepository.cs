using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FinancialPlannerApplication.Models.DataAccess
{
    public class FinancialPlannerRepository : IFinancialPlannerRepository
    {
        private readonly FinancialPlannerDbContext Db;

        public FinancialPlannerRepository()
        {
            Db = new FinancialPlannerDbContext();
        }

        public IEnumerable<Account> GetAccounts()
        {
            return Db.Accounts;
        }

        public IEnumerable<Budget> GetBudgets()
        {
            return Db.Budgets;
        }

        public IEnumerable<BudgetItem> GetBudgetItems()
        {
            return Db.BudgetItems;
        }
      
        public IEnumerable<Transaction> GetTransactions()
        {
            return Db.Transactions;
        }

        public IEnumerable<Expense> GetExpenses()
        {
            return Db.Expenses;
        } 

        public void AddAccount(Account account)
        {
            var entry = Db.Entry(account);

            if (entry.State != EntityState.Detached)
                entry.State = EntityState.Added;
            else
                Db.Accounts.Add(account);
        }

        public void EditAccount(Account account)
        {
            var existing = Db.Accounts.FirstOrDefault(m => m.Id == account.Id);

            if(existing != null)
                Db.Entry(existing).State = EntityState.Detached;

            Db.Accounts.Attach(account);
            Db.Entry(account).State = EntityState.Modified;
        }

        public void AddTransaction(Transaction transaction)
        {
            var entry = Db.Entry(transaction);

            if (entry.State != EntityState.Detached)
                entry.State = EntityState.Added;
            else
                Db.Transactions.Add(transaction);
        }

        public void EditTransaction(Transaction transaction)
        {
            var existing = Db.Transactions.FirstOrDefault(m => m.Id == transaction.Id);

            if (existing != null)
                Db.Entry(existing).State = EntityState.Detached;

            Db.Transactions.Attach(transaction);
            Db.Entry(transaction).State = EntityState.Modified;
        }

        public void AddBudget(Budget budget)
        {
            var entry = Db.Entry(budget);

            if (entry.State != EntityState.Detached)
                entry.State = EntityState.Added;
            else
                Db.Budgets.Add(budget);
        }

        public void EditBudget(Budget budget)
        {
            var existing = Db.Transactions.FirstOrDefault(m => m.Id == budget.Id);

            if (existing != null)
                Db.Entry(existing).State = EntityState.Detached;

            Db.Budgets.Attach(budget);
            Db.Entry(budget).State = EntityState.Modified;
        }

        public void EditBudgetItem(BudgetItem budgetItem)
        {
            var existing = Db.BudgetItems.FirstOrDefault(m => m.Id == budgetItem.Id);

            if (existing != null)
                Db.Entry(existing).State = EntityState.Detached;

            Db.BudgetItems.Attach(budgetItem);
            Db.Entry(budgetItem).State = EntityState.Modified;
        }

        public void AddBudgetItem(BudgetItem budgetItem)
        {
            var entry = Db.Entry(budgetItem);

            if (entry.State != EntityState.Detached)
                entry.State = EntityState.Added;
            else
                Db.BudgetItems.Add(budgetItem);
        }

        public void AddExpense(Expense expense)
        {
            var entry = Db.Entry(expense);

            if (entry.State != EntityState.Detached)
                entry.State = EntityState.Added;
            else
                Db.Expenses.Add(expense);
        }

        public void EditExpense(Expense expense)
        {
            var existing = Db.Expenses.FirstOrDefault(m => m.Id == expense.Id);

            if (existing != null)
                Db.Entry(existing).State = EntityState.Detached;

            Db.Expenses.Attach(expense);
            Db.Entry(expense).State = EntityState.Modified;
        }

        public void Save()
        {
            Db.SaveChanges();
        }
    }
}