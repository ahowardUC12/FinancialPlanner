using System;
using System.Collections.Generic;
using System.Linq;
using FinancialPlannerApplication.Models.DataAccess;
using FinancialPlannerApplication.Models.ViewModels;

namespace FinancialPlannerApplication.Models.Services
{
    public class HomeService : IHomeService
    {
        private readonly IFinancialPlannerRepository FinancialPlannerRepository;

        public HomeService(IFinancialPlannerRepository financialPlannerRepository)
        {
            FinancialPlannerRepository = financialPlannerRepository;
        }

        public HomeIndexViewModel MapHomeIndexViewModel(string username)
        {
            var vm = new HomeIndexViewModel();
            var accounts = FinancialPlannerRepository.GetAccounts().Where(m => m.UserName == username);
            var budgets = FinancialPlannerRepository.GetBudgets().Where(m => m.Username == username).ToList();
            SetBudgetsAndAccounts(vm, budgets, accounts);

            SetSelectedAccountId(vm);

            var selectedBudget = budgets.FirstOrDefault();

            if (selectedBudget != null)
            {
                SetBudgetId(vm, selectedBudget);
                var expenses = FinancialPlannerRepository.GetExpenses().Where(m => m.Username == username).ToList();
                SetExpenseProgress(vm, expenses);
            }

            SetFromAndToDates(vm);

            return vm;
        }

        private static void SetSelectedAccountId(HomeIndexViewModel vm)
        {
            var selectedAccount = vm.Accounts.FirstOrDefault();

            if (selectedAccount != null)
                vm.SelectedAccountId = selectedAccount.Id;
        }

        private static void SetBudgetId(HomeIndexViewModel vm, Budget selectedBudget)
        {
            vm.SelectedBudgetId = selectedBudget.Id;
        }

        private static void SetFromAndToDates(HomeIndexViewModel vm)
        {
            vm.FromDate = DateTime.Now.AddYears(-1);
            vm.ToDate = DateTime.Now;
        }

        public IEnumerable<BudgetItemTotalsViewModel> GetBudgetItemTotals(string username, int budgetId)
        {
            var budgetItemTotals = new List<BudgetItemTotalsViewModel>();
            var budgets = FinancialPlannerRepository.GetBudgets().Where(m => m.Username == username);
            var selectedBudget = budgetId == 0 ? budgets.FirstOrDefault() : budgets.FirstOrDefault(m => m.Id == budgetId);

            if (selectedBudget != null)
            {
                var budgetItems = FinancialPlannerRepository.GetBudgetItems()
                    .Where(m => m.BudgetId == selectedBudget.Id).ToList();

                budgetItemTotals = budgetItems.Select(m => new BudgetItemTotalsViewModel
                {
                    Amount = m.Amount,
                    Name = m.Name,
                    BudgetName = selectedBudget.Name
                }).ToList();
            }

            return budgetItemTotals;
        }

        public IEnumerable<BudgetItemTransactionTotalsViewModel> GetBudgetTotalsViewModel(string username, int budgetId)
        {
            var budgetTotalsVm = new List<BudgetItemTransactionTotalsViewModel>();
            var budgets = FinancialPlannerRepository.GetBudgets().Where(m => m.Username == username);
            var selectedBudget = budgetId == 0 ? budgets.FirstOrDefault() : FinancialPlannerRepository.GetBudgets().FirstOrDefault(m => m.Id == budgetId);

            if (selectedBudget == null) 
                return budgetTotalsVm;

            var budgetItems = FinancialPlannerRepository.GetBudgetItems()
                .Where(m => m.BudgetId == selectedBudget.Id).ToList();

            budgetTotalsVm.AddRange(from item in budgetItems
                let itemId = item.Id
                let itemTransactions = FinancialPlannerRepository.GetTransactions().Where(m => m.BudgetItemId == itemId).ToList()
                let withdrawalAmount = itemTransactions.Where(m => m.IsWithdrawal).Sum(m => m.Amount)
                let depositAmount = itemTransactions.Where(m => !m.IsWithdrawal).Sum(m => m.Amount)
                let total = withdrawalAmount + depositAmount
                select new BudgetItemTransactionTotalsViewModel
                {
                    Name = item.Name, 
                    Amount = total,
                    BudgetName = selectedBudget.Name
                });

            return budgetTotalsVm;
        }

        public IEnumerable<TransactionTotalsViewModel> GetAccountProgressViewModels(int accountId, DateTime? fromDate,
            DateTime? toDate)
        {
            var accountTransactions =
               FinancialPlannerRepository.GetTransactions().Where(n => n.AccountId == accountId)
                   .Where(m => m.PaymentDate >= fromDate && m.PaymentDate <= toDate).ToList();

            var transactions = accountTransactions.Select(m => new TransactionTotalsViewModel
            {
                Amount = m.Amount,
                Day = m.PaymentDate.Day,
                Month = m.PaymentDate.Month,
                Year = m.PaymentDate.Year
            }).ToList();

            var transactionsTotalsByDate = transactions.GroupBy(m => new { m.Day, m.Month, m.Year }, m => m.Amount,
                (date, trans) => new TransactionTotalsViewModel
                {
                    Amount = trans.Sum(),
                    Day = date.Day,
                    Month = date.Month,
                    Year = date.Year,
                }).ToList();

            foreach (var total in transactionsTotalsByDate)
            {
                var day = total.Day;
                var month = total.Month;
                var year = total.Year;
                var transForDate =
                    accountTransactions.Where(
                        m => m.PaymentDate.Day == day && m.PaymentDate.Month == month && m.PaymentDate.Year == year).ToList();

                var tranDate = transForDate.Max(m => m.PaymentDate);

                var tran = transForDate.FirstOrDefault(m => m.PaymentDate == tranDate);

                if (tran != null)
                    total.Balance = tran.Balance;
            }

            var transactionTotals = transactionsTotalsByDate.OrderBy(m => m.Year).ThenBy(m => m.Month).ThenBy(m => m.Day);

            return transactionTotals;
        }

        public IEnumerable<BudgetProgessViewModel> GetBudgetProgress(string username, int budgetId)
        {
            if (budgetId <= 0)
            {
                var budget =
                    FinancialPlannerRepository.GetBudgets().FirstOrDefault(m => m.Username == username);

                if(budget != null)
                    budgetId = budget.Id;
            }

            var budgetItems = FinancialPlannerRepository.GetBudgetItems().Where(m => m.BudgetId == budgetId);

            var budgetProgress = budgetItems.Select(m => new BudgetProgessViewModel
            {
                Name = m.Name,
                AmountSpent = m.Amount - m.Balance,
                AmountLeft = m.Balance,
                AmountSpentPerc = ((m.Amount - m.Balance) / m.Amount) * 100,
                AmountLeftPerc = (m.Balance / m.Amount) * 100
            });

            return budgetProgress;
        }

        private static void SetExpenseProgress(HomeIndexViewModel vm, IEnumerable<Expense> expenses)
        {
            var expenseProgesses = expenses.Select(e => new ExpenseProgessViewModel
            {
                Name = e.Name,
                AmountLeft = e.Balance,
                AmountSpent = e.Amount - e.Balance,
                AmountLeftPerc = (e.Balance / e.Amount) * 100,
                AmountSpentPerc = ((e.Amount - e.Balance) / e.Amount) * 100
            }).ToList();

            vm.ExpenseProgess = expenseProgesses;
        }

        private static void SetBudgetsAndAccounts(HomeIndexViewModel vm, IEnumerable<Budget> budgets, IEnumerable<Account> accounts)
        {
            vm.Budgets = budgets.Select(m => new BudgetViewModel
            {
                Id = m.Id,
                StartDate = m.StartDate,
                EndDate = m.EndDate,
                Name = m.Name
            }).ToList();
            vm.Accounts = accounts.Select(m => new AccountViewModel
            {
                Id = m.Id,
                Amount = m.Amount,
                Name = m.Name,
                UserName = m.UserName
            }).ToList();
        }
    }
}