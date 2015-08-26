using System.Collections.Generic;
using System.Linq;
using FinancialPlannerApplication.Models.ViewModels;

namespace FinancialPlannerApplication.Models.Services
{
    public class SetViewModelService : ISetViewModelsService
    {
        public List<AccountViewModel> SetAccountViewModels(IEnumerable<Account> accounts)
        {
            return accounts.Select(m => new AccountViewModel
            {
                Id = m.Id,
                Amount = m.Amount,
                Name = m.Name,
                UserName = m.UserName
            }).ToList();
        }

        public List<ExpenseViewModel> SetExpenseViewModels(IEnumerable<Expense> expenses)
        {
            return expenses.Select(m => new ExpenseViewModel
            {
                Id = m.Id,
                Amount = m.Amount,
                Balance = m.Balance,
                InterestRate = m.InterestRate,
                Name = m.Name,
                Username = m.Username
            }).ToList();
        }

        public List<BudgetViewModel> SetBudgetViewModels(IEnumerable<Budget> budgets)
        {
            return budgets.Select(m => new BudgetViewModel
            {
                Id = m.Id,
                Name = m.Name,
                StartDate = m.StartDate,
                EndDate = m.EndDate
            }).ToList();
        }

        public List<BudgetItemViewModel> SetBudgetItemViewModels(IEnumerable<BudgetItem> budgetItems)
        {
            return budgetItems.Select(m => new BudgetItemViewModel
            {
                Id = m.Id,
                Amount = m.Amount,
                BudgetId = m.BudgetId,
                Name = m.Name,
                Balance = m.Balance
            }).ToList();
        }

        public List<TransactionViewModel> SetTransactionViewModels(IEnumerable<Transaction> transactions)
        {
            return transactions.Select(m => new TransactionViewModel
            {
                Id = m.Id,
                Amount = m.Amount,
                Name = m.Name,
                PaymentDate = m.PaymentDate,
                IsWithdrawal = m.IsWithdrawal,
                Balance = m.Balance
            }).OrderBy(m => m.PaymentDate).ToList();
        }


    }
}