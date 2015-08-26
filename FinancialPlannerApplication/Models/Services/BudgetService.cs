using System;
using System.Collections.Generic;
using System.Linq;
using FinancialPlannerApplication.Models.DataAccess;
using FinancialPlannerApplication.Models.ViewModels;

namespace FinancialPlannerApplication.Models.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IFinancialPlannerRepository FinancialPlannerRepository;
        private readonly ISetViewModelsService SetViewModelsService;
        private readonly ISharedOperationsService SharedOperationsService;

        public BudgetService (IFinancialPlannerRepository financialPlannerRepository, ISetViewModelsService setViewModelsService,
            ISharedOperationsService sharedOperationsService)
        {
            FinancialPlannerRepository = financialPlannerRepository;
            SetViewModelsService = setViewModelsService;
            SharedOperationsService = sharedOperationsService;
        }
        public BudgetIndexViewModel MapBudgetIndexViewModel(string username)
        {
            var budgets = FinancialPlannerRepository.GetBudgets()
               .Where(m => m.Username == username);

            var vm = new BudgetIndexViewModel
            {
                Budgets = SetViewModelsService.SetBudgetViewModels(budgets)
            };

            return vm;
        }

        public Budget AddBudget(EditBudgetViewModel vm, string username)
        {
            var budget = new Budget
            {
                Name = vm.Name,
                StartDate = vm.StartDate,
                EndDate = vm.EndDate,
                Username = username
            };

            FinancialPlannerRepository.AddBudget(budget);
            FinancialPlannerRepository.Save();

            return budget;
        }

        public bool EditBudget(EditBudgetViewModel vm)
        {
            var budget = FinancialPlannerRepository.GetBudgets().FirstOrDefault(m => m.Id == vm.Id);

            if (budget == null)
                return false;

            budget.Name = vm.Name;
            budget.StartDate = vm.StartDate;
            budget.EndDate = vm.EndDate;

            FinancialPlannerRepository.Save();

            return true;
        }

        public EditBudgetItemViewModel MapEditBudgetItemViewModelForAdd(string username)
        {
            var budgets = FinancialPlannerRepository.GetBudgets()
               .Where(m => m.Username == username);

            var vm = new EditBudgetItemViewModel
            {
                Budgets = SetViewModelsService.SetBudgetViewModels(budgets)
            };

            return vm;
        }

        public BudgetItem AddBudgetItem(EditBudgetItemViewModel vm)
        {
            var budgetItem = new BudgetItem
            {
                Name = vm.Name,
                Amount = vm.Amount,
                BudgetId = vm.SelectedBudgetId,
                Balance = vm.Amount
            };

            FinancialPlannerRepository.AddBudgetItem(budgetItem);
            FinancialPlannerRepository.Save();

            return budgetItem;
        }

        public bool EditBudgetItem(EditBudgetItemViewModel vm)
        {
            var budgetItem = FinancialPlannerRepository.GetBudgetItems().FirstOrDefault(m => m.Id == vm.BudgetItemId);

            if (budgetItem == null)
                return false;

            var oldBudgetBalance = budgetItem.Balance;
            var oldBudgetAmount = budgetItem.Amount;

            budgetItem.Name = vm.Name;
            budgetItem.Amount = vm.Amount;
            budgetItem.BudgetId = vm.SelectedBudgetId;
            budgetItem.Balance = oldBudgetBalance + (vm.Amount - oldBudgetAmount);

            FinancialPlannerRepository.Save();

            return true;
        }

        public EditBudgetItemTransactionViewModel MapEditBudgetItemTransactionViewModelForAdd(string username, int budgetItemId)
        {
            var accounts = FinancialPlannerRepository.GetAccounts()
                .Where(m => m.UserName == username);

            var selectedBudgetItem =
                FinancialPlannerRepository.GetBudgetItems().FirstOrDefault(m => m.Id == budgetItemId);

            if (selectedBudgetItem == null)
                return null;

            var budgets = FinancialPlannerRepository.GetBudgets().Where(m => m.Username == username).ToList();
            budgets = budgets.Where(m => m.BudgetItems != null && m.BudgetItems.Any()).ToList();
            var budgetItems = FinancialPlannerRepository.GetBudgetItems().Where(m => m.BudgetId == selectedBudgetItem.BudgetId);
            var expenses = FinancialPlannerRepository.GetExpenses().Where(m => m.Username == username);

            var vm = new EditBudgetItemTransactionViewModel
            {
                PaymentDate = DateTime.Now,
                BudgetItems = SetViewModelsService.SetBudgetItemViewModels(budgetItems),
                Accounts = SetViewModelsService.SetAccountViewModels(accounts),
                Budgets = SetViewModelsService.SetBudgetViewModels(budgets),
                Expenses = SetViewModelsService.SetExpenseViewModels(expenses),
                SelectedBudgetId = selectedBudgetItem.BudgetId,
                SelectedBudgetItemId = selectedBudgetItem.Id
            };

            return vm;
        }

        public EditBudgetItemTransactionViewModel MapEditBudgetItemTransactionViewModelChangeBudget(string username,
            EditBudgetItemTransactionViewModel vm)
        {
            var accounts = FinancialPlannerRepository.GetAccounts()
                .Where(m => m.UserName == username);
            var budgets = FinancialPlannerRepository.GetBudgets().Where(m => m.Username == username).ToList();

            budgets = budgets.Where(m => m.BudgetItems != null && m.BudgetItems.Any()).ToList();
            var budgetItems = FinancialPlannerRepository.GetBudgetItems().Where(m => m.BudgetId == vm.SelectedBudgetId).ToList();
            var expenses = FinancialPlannerRepository.GetExpenses().Where(m => m.Username == username);

            vm.PaymentDate = DateTime.Now;
            vm.BudgetItems = SetViewModelsService.SetBudgetItemViewModels(budgetItems);
            vm.Accounts = SetViewModelsService.SetAccountViewModels(accounts);
            vm.Budgets = SetViewModelsService.SetBudgetViewModels(budgets);
            vm.Expenses = SetViewModelsService.SetExpenseViewModels(expenses);

            var selectedBudgetItem = budgetItems.Any() ? budgetItems.FirstOrDefault() : null;

            if (selectedBudgetItem != null)
                vm.SelectedBudgetItemId = selectedBudgetItem.Id;

            return vm;
        }

        public Transaction AddBudgetItemTransaction(EditBudgetItemTransactionViewModel vm)
        {
            var budgetItemTransaction = new Transaction
            {
                Name = vm.Name,
                AccountId = vm.SelectedAccountId == -1 ? null : vm.SelectedAccountId,
                ExpenseId = vm.SelectedExpenseId == -1 ? null : vm.SelectedExpenseId,
                Amount = vm.Amount,
                BudgetItemId = vm.SelectedBudgetItemId,
                IsWithdrawal = vm.IsWithdrawal,
                PaymentDate = vm.PaymentDate
            };

            FinancialPlannerRepository.AddTransaction(budgetItemTransaction);
            FinancialPlannerRepository.Save();

            return budgetItemTransaction;
        }

        public bool EditBudgetItemForAdd(EditBudgetItemTransactionViewModel vm)
        {
            var budgetItem =
               FinancialPlannerRepository.GetBudgetItems().FirstOrDefault(m => m.Id == vm.SelectedBudgetItemId);

            if (budgetItem == null)
                return false;

            budgetItem.Balance = vm.IsWithdrawal
              ? budgetItem.Balance - vm.Amount
              : budgetItem.Balance + vm.Amount;

            FinancialPlannerRepository.Save();

            return true;
        }

        public void EditAccountAmount(EditBudgetItemTransactionViewModel vm, Account account, IList<Transaction> transactions)
        {
            SharedOperationsService.AdjustAccountAmount(account);
        }

        public void EditAccountTransactionBalances(EditBudgetItemTransactionViewModel vm, Account account,
            IList<Transaction> transactions)
        {
            SharedOperationsService.AdjustTransactionBalances(vm.SelectedAccountId, account);
        }

        public BudgetDetailsViewModel MapBudgetDetailsViewModel(int id)
        {
            var budgetDetails = FinancialPlannerRepository.GetBudgetItems().Where(m => m.BudgetId == id);
            var budget = FinancialPlannerRepository.GetBudgets().Select(m => new BudgetViewModel
            {
                Id = m.Id,
                Name = m.Name
            }).FirstOrDefault(m => m.Id == id);

            var vm = new BudgetDetailsViewModel
            {
                BudgetItems = SetViewModelsService.SetBudgetItemViewModels(budgetDetails),
                Budget = budget
            };

            return vm;
        }

        public BudgetItemTransactionViewModel MapBudgetItemTransactionViewModel(int id)
        {
            var transactions = FinancialPlannerRepository.GetTransactions()
                .Where(m => m.BudgetItemId == id).ToList();

            var withdrawalAmount = transactions.Where(m => m.IsWithdrawal).Sum(m => m.Amount);
            var depositAmount = transactions.Where(m => !m.IsWithdrawal).Sum(m => m.Amount);

            var transactionTotal = depositAmount - withdrawalAmount;

            var vm = new BudgetItemTransactionViewModel
            {
                BudgetItemTransactions = SetViewModelsService.SetTransactionViewModels(transactions),
                BudgetItemId = id,
                TransactionTotal = transactionTotal
            };

            return vm;
        }

        public EditBudgetViewModel MapEditBudgetViewModel(int id)
        {
            var budget = FinancialPlannerRepository.GetBudgets()
               .FirstOrDefault(m => m.Id == id);

            if (budget == null)
                return null;

            var vm = new EditBudgetViewModel
            {
                Id = budget.Id,
                Name = budget.Name,
                StartDate = budget.StartDate,
                EndDate = budget.EndDate
            };

            return vm;
        }

        public EditBudgetItemViewModel MapEditBudgetItemViewModel(int id, string username)
        {
            var budgetItem = FinancialPlannerRepository.GetBudgetItems()
               .FirstOrDefault(m => m.Id == id);
            var budgets = FinancialPlannerRepository.GetBudgets()
                .Where(m => m.Username == username);

            if (budgetItem == null)
                return null;

            var vm = new EditBudgetItemViewModel
            {
                BudgetItemId = budgetItem.Id,
                Name = budgetItem.Name,
                Amount = budgetItem.Amount,
                SelectedBudgetId = budgetItem.BudgetId,
                Budgets = budgets.Select(m => new BudgetViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate
                }).ToList()
            };

            return vm;
        }

        public EditBudgetItemTransactionViewModel MapEditBudgetItemTransactionViewModelForEdit(int id, string username)
        {
            var budgetItems = new List<BudgetItem>();

            var budgetItemTransaction = FinancialPlannerRepository.GetTransactions()
                .FirstOrDefault(m => m.Id == id);

            if (budgetItemTransaction == null)
                return null;

            var accounts = FinancialPlannerRepository.GetAccounts()
              .Where(m => m.UserName == username);

            var budgets = FinancialPlannerRepository.GetBudgets()
                .Where(m => m.Username == username).ToList();

            budgets = budgets.Where(m => m.BudgetItems != null && m.BudgetItems.Any()).ToList();

            var expenses = FinancialPlannerRepository.GetExpenses().Where(m => m.Username == username);

            var budgetItemForTransaction = FinancialPlannerRepository.GetBudgetItems()
                .FirstOrDefault(m => m.Id == budgetItemTransaction.BudgetItemId);

            if (budgetItemForTransaction != null)
            {
                budgetItems = FinancialPlannerRepository.GetBudgetItems()
                    .Where(m => m.BudgetId == budgetItemForTransaction.BudgetId).ToList();
            }

            var vm = new EditBudgetItemTransactionViewModel
            {
                BudgetItemTransactionId = budgetItemTransaction.Id,
                Name = budgetItemTransaction.Name,
                Amount = budgetItemTransaction.Amount,
                PaymentDate = budgetItemTransaction.PaymentDate < DateTime.Now.AddYears(-50) ?
                                budgetItemTransaction.PaymentDate : DateTime.Now,
                OldAmount = budgetItemTransaction.Amount,
                OldAccountId = budgetItemTransaction.AccountId,
                WasWithdrawal = budgetItemTransaction.IsWithdrawal,
                Accounts = SetViewModelsService.SetAccountViewModels(accounts),
                BudgetItems = SetViewModelsService.SetBudgetItemViewModels(budgetItems),
                Budgets = SetViewModelsService.SetBudgetViewModels(budgets),
                Expenses = SetViewModelsService.SetExpenseViewModels(expenses),
                SelectedBudgetId = budgetItemForTransaction == null ? -1 : budgetItemForTransaction.BudgetId,
                SelectedBudgetItemId = budgetItemForTransaction == null ? -1 : budgetItemForTransaction.Id,
                IsWithdrawal = budgetItemTransaction.IsWithdrawal
            };

            var account = FinancialPlannerRepository.GetAccounts()
                               .FirstOrDefault(m => m.Id == budgetItemTransaction.AccountId);

            if (account != null)
                vm.SelectedAccountId = account.Id;

            var expense = FinancialPlannerRepository.GetExpenses()
                .FirstOrDefault(m => m.Id == budgetItemTransaction.ExpenseId);

            if (expense != null)
                vm.SelectedExpenseId = expense.Id;


            return vm;
        }

        public EditBudgetItemTransactionViewModel MapEditBudgetItemTransactionViewModelForChange(EditBudgetItemTransactionViewModel vm, string username)
        {
            var budgetItems = FinancialPlannerRepository.GetBudgetItems().Where(m => m.BudgetId == vm.SelectedBudgetId).ToList();

            var budgetItemTransaction = FinancialPlannerRepository.GetTransactions()
                .FirstOrDefault(m => m.Id == vm.BudgetItemTransactionId);

            if (budgetItemTransaction == null)
                return null;

            var accounts = FinancialPlannerRepository.GetAccounts()
              .Where(m => m.UserName == username);

            var budgets = FinancialPlannerRepository.GetBudgets()
                .Where(m => m.Username == username).ToList();

            budgets = budgets.Where(m => m.BudgetItems != null && m.BudgetItems.Any()).ToList();

            var expenses = FinancialPlannerRepository.GetExpenses().Where(m => m.Username == username);

            vm.BudgetItems = SetViewModelsService.SetBudgetItemViewModels(budgetItems);
            vm.Budgets = SetViewModelsService.SetBudgetViewModels(budgets);
            vm.Accounts = SetViewModelsService.SetAccountViewModels(accounts);
            vm.Expenses = SetViewModelsService.SetExpenseViewModels(expenses);

            var selectedBudgetItem = budgetItems.FirstOrDefault();
            vm.SelectedBudgetItemId = selectedBudgetItem != null ? selectedBudgetItem.Id : vm.SelectedBudgetItemId;

            return vm;
        }

        public void EditBudgetTransaction(Transaction transaction, EditBudgetItemTransactionViewModel vm)
        {
            transaction.Amount = vm.Amount;
            transaction.AccountId = vm.SelectedAccountId == -1 ? null : vm.SelectedAccountId;
            transaction.ExpenseId = vm.SelectedExpenseId == -1 ? null : vm.SelectedExpenseId;
            transaction.BudgetItemId = vm.SelectedBudgetItemId;
            transaction.IsWithdrawal = vm.IsWithdrawal;
            transaction.PaymentDate = vm.PaymentDate;
            transaction.Name = vm.Name;

            FinancialPlannerRepository.EditTransaction(transaction);
            FinancialPlannerRepository.Save();
        }
        
        public void EditBudgetItemBalance(BudgetItem budgetItem, Transaction transaction)
        {
            SharedOperationsService.AdjustBudgetItemBalance(budgetItem, transaction.IsWithdrawal, transaction.Amount);
        }
     
        public void EditBudgetItemBalance(BudgetItem budgetItem)
        {
            SharedOperationsService.EditBudgetItemBalance(budgetItem);
        }

        public void AdjustExpenseBalance(Expense expense, Transaction transaction)
        {
            SharedOperationsService.AdjustExpenseBalance(expense, transaction);
        }

        public void AdjustExpenseBalance(Expense expense)
        {
            SharedOperationsService.AdjustExpenseBalance(expense);
        }
    }
}