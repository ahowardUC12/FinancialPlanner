using System;
using System.Linq;
using FinancialPlannerApplication.Models.DataAccess;
using FinancialPlannerApplication.Models.ViewModels;

namespace FinancialPlannerApplication.Models.Services
{
    public class FinancialPlannerAccountService : IFinancialPlannerAccountService
    {
        private readonly IFinancialPlannerRepository FinancialPlannerRepository;
        private readonly ISetViewModelsService SetViewModelsService;
        private readonly ISharedOperationsService SharedOperationsService;

        public FinancialPlannerAccountService(IFinancialPlannerRepository financialPlannerRepository, ISetViewModelsService setViewModelsService,
            ISharedOperationsService sharedOperationsService)
        {
            FinancialPlannerRepository = financialPlannerRepository;
            SetViewModelsService = setViewModelsService;
            SharedOperationsService = sharedOperationsService;
        }

        public AccountIndexViewModel MapAccountIndexViewModel(string username)
        {
            var accounts = FinancialPlannerRepository.GetAccounts()
              .Where(m => m.UserName == username);

            var vm = new AccountIndexViewModel
            {
                Accounts = SetViewModelsService.SetAccountViewModels(accounts)
            };

            return vm;
        }

        public EditAccountViewModel MapEditAccountViewModel(int id)
        {
            var account = FinancialPlannerRepository.GetAccounts()
                    .FirstOrDefault(m => m.Id == id);

            if (account == null)
                return null;

            var vm = new EditAccountViewModel
            {
                Id = account.Id,
                Name = account.Name,
                Amount = account.Amount,
                UserName = account.UserName
            };

            return vm;
        }

        public Account AddAccount(string username, EditAccountViewModel vm)
        {
            var account = new Account
            {
                Name = vm.Name,
                Amount = vm.Amount,
                UserName = username,
                InitialAmount = vm.Amount
            };

            FinancialPlannerRepository.AddAccount(account);
            FinancialPlannerRepository.Save();

            return account;
        }

        public EditTransactionViewModel MapEditTransactionViewModel(string username)
        {
            var accounts = FinancialPlannerRepository.GetAccounts()
              .Where(m => m.UserName == username);

            var vm = new EditTransactionViewModel
            {
                Accounts = SetViewModelsService.SetAccountViewModels(accounts)
            };

            return vm;
        }

        public EditTransactionViewModel MapEditTransactionViewModelForEdit(int id, int accountId, string username)
        {
            var transaction = FinancialPlannerRepository.GetTransactions()
              .FirstOrDefault(m => m.Id == id);

            if (transaction == null)
                return null;

            var accounts = FinancialPlannerRepository.GetAccounts()
                .Where(m => m.UserName == username);

            var budgets = FinancialPlannerRepository.GetBudgets()
                .Where(m => m.Username == username).ToList();

            var expenses = FinancialPlannerRepository.GetExpenses().Where(m => m.Username == username);

            var vm = new EditTransactionViewModel
            {
                Accounts = SetViewModelsService.SetAccountViewModels(accounts),
                AccountTransactionId = transaction.Id,
                Amount = transaction.Amount,
                IsWithdrawal = transaction.IsWithdrawal,
                Name = transaction.Name,
                PaymentDate = transaction.PaymentDate,
                SelectedAccountId = accountId != 0 ? accountId : 0,
                OldAccountId = accountId != 0 ? accountId : 0,
                WasWithdrawal = transaction.IsWithdrawal,
                OldAmount = transaction.Amount,
                Budgets = SetViewModelsService.SetBudgetViewModels(budgets),
                Expenses = SetViewModelsService.SetExpenseViewModels(expenses)
            };

            if(transaction.BudgetItemId != null && transaction.BudgetItemId > 0)
                SetBudgetItemsForViewModel(transaction, vm);

            SetSelectedExpenseForViewModel(transaction, vm);

            return vm;
        }

        private void SetSelectedExpenseForViewModel(Transaction transaction, EditTransactionViewModel vm)
        {
            var selectedExpense =
                FinancialPlannerRepository.GetExpenses().FirstOrDefault(m => m.Id == transaction.ExpenseId);

            if (selectedExpense != null)
            {
                vm.SelectedExpenseId = selectedExpense.Id;
            }
        }

        private void SetBudgetItemsForViewModel(Transaction transaction, EditTransactionViewModel vm)
        {
            var selectedBudgetItem =
                FinancialPlannerRepository.GetBudgetItems().FirstOrDefault(m => m.Id == transaction.BudgetItemId);

            if (selectedBudgetItem != null)
            {
                var budgetItems =
                    FinancialPlannerRepository.GetBudgetItems()
                        .Where(m => m.BudgetId == selectedBudgetItem.BudgetId)
                        .ToList();

                vm.BudgetItems = SetViewModelsService.SetBudgetItemViewModels(budgetItems);
                vm.SelectedBudgetId = selectedBudgetItem.BudgetId;
                vm.SelectedBudgetItemId = transaction.BudgetItemId;
            }
        }

        public bool EditAccount(EditAccountViewModel vm, string username)
        {
            var account = FinancialPlannerRepository.GetAccounts().FirstOrDefault(m => m.Id == vm.Id);

            if (account == null)
                return false;

            account.InitialAmount = account.InitialAmount + (vm.Amount - account.Amount);
            account.Name = vm.Name;
            account.Amount = vm.Amount;
            account.UserName = username;
            
            FinancialPlannerRepository.EditAccount(account);
            FinancialPlannerRepository.Save();

            AdjustTransactionBalances(account);

            return true;
        }

        public AccountTransactionsViewModel MapAccountTransactionsViewModel(int id)
        {
            var transactions = FinancialPlannerRepository.GetTransactions()
               .Where(m => m.AccountId == id).ToList();

            var withdrawalAmount = transactions.Where(m => m.IsWithdrawal).Sum(m => m.Amount);
            var depositAmount = transactions.Where(m => !m.IsWithdrawal).Sum(m => m.Amount);

            var transactionTotal = depositAmount - withdrawalAmount;

            var vm = new AccountTransactionsViewModel
            {
                Transactions = SetViewModelsService.SetTransactionViewModels(transactions),
                TransactionTotal = transactionTotal
            };

            return vm;
        }

        public EditTransactionViewModel MapEditTransactionViewModelForAdd(int id, string username)
        {
            var accounts = FinancialPlannerRepository.GetAccounts()
               .Where(m => m.UserName == username);
            var budgets = FinancialPlannerRepository.GetBudgets()
                .Where(m => m.Username == username).ToList();
            var expenses = FinancialPlannerRepository.GetExpenses().Where(m => m.Username == username);
            budgets = budgets.Where(m => m.BudgetItems.Any()).ToList();

            var vm = new EditTransactionViewModel
            {
                Accounts = SetViewModelsService.SetAccountViewModels(accounts),
                Budgets = budgets.Any() ? SetViewModelsService.SetBudgetViewModels(budgets) : null,
                SelectedAccountId = id != 0 ? id : 0,
                PaymentDate = DateTime.Now,
                NewTransaction = true,
                Expenses = SetViewModelsService.SetExpenseViewModels(expenses)
            };

            return vm;
        }

        public void AdjustAccountBalance(Account account, EditTransactionViewModel vm)
        {
            SharedOperationsService.AdjustAccountBalance(account, vm.IsWithdrawal, vm.Amount);
        }

        public Transaction AddTransaction(Account account, EditTransactionViewModel vm)
        {
            var newTransaction = new Transaction
            {
                Name = vm.Name,
                Amount = vm.Amount,
                IsWithdrawal = vm.IsWithdrawal,
                AccountId = vm.SelectedAccountId != -1 ? vm.SelectedAccountId : 0,
                PaymentDate = vm.PaymentDate,
                Balance = account.Amount,
                BudgetItemId = vm.SelectedBudgetItemId,
                ExpenseId = vm.SelectedExpenseId != -1 ? vm.SelectedExpenseId : 0
            };

            FinancialPlannerRepository.AddTransaction(newTransaction);
            FinancialPlannerRepository.Save();

            return newTransaction;
        }

        public void AdjustBudgetItemBalance(BudgetItem budgetItem, EditTransactionViewModel vm)
        {
            SharedOperationsService.AdjustBudgetItemBalance(budgetItem, vm.IsWithdrawal, vm.Amount);
        }

        public void AdjustTransactionBalances(Account account, EditTransactionViewModel vm)
        {
            SharedOperationsService.AdjustTransactionBalances(vm.SelectedAccountId, account);
        }

        public void MapEditTransactionViewModel(EditTransactionViewModel vm, string username)
        {
            var accounts = FinancialPlannerRepository.GetAccounts().Where(m => m.UserName == username);
            var budgets = FinancialPlannerRepository.GetBudgets().Where(m => m.Username == username);
            var expenses = FinancialPlannerRepository.GetExpenses().Where(m => m.Username == username);
            var budgetItems =
                FinancialPlannerRepository.GetBudgetItems().Where(m => m.BudgetId == vm.SelectedBudgetId);
            vm.BudgetItems = SetViewModelsService.SetBudgetItemViewModels(budgetItems);
            vm.Accounts = SetViewModelsService.SetAccountViewModels(accounts);
            vm.Budgets = SetViewModelsService.SetBudgetViewModels(budgets);
            vm.Expenses = SetViewModelsService.SetExpenseViewModels(expenses);
        }

        public void EditTransaction(EditTransactionViewModel vm)
        {
            var transaction = FinancialPlannerRepository.GetTransactions().FirstOrDefault(m => m.Id == vm.AccountTransactionId);

            if (transaction == null) return;

            transaction.Amount = vm.Amount;
            transaction.Name = vm.Name;
            transaction.PaymentDate = vm.PaymentDate;
            transaction.IsWithdrawal = vm.IsWithdrawal;
            transaction.BudgetItemId = vm.SelectedBudgetItemId == -1 ? null : vm.SelectedBudgetItemId;
            transaction.AccountId = vm.SelectedAccountId;
            transaction.ExpenseId = vm.SelectedExpenseId == -1 ? null : vm.SelectedExpenseId;

            FinancialPlannerRepository.Save();
        }

        public void AdjustOldBudgetItemBalance(int? oldBudgetItemId)
        {
            SharedOperationsService.AdjustOldBudgetItemBalance(oldBudgetItemId);
        }

        public void AdjustNewBudgetItemBalance(EditTransactionViewModel vm)
        {
            SharedOperationsService.AdjustNewBudgetItemBalance(vm.SelectedBudgetItemId);
        }

        private void EditBudgetItem(BudgetItem budgetItem)
        {
            SharedOperationsService.EditBudgetItemBalance(budgetItem);
        }

        public void AdjustTransactionBalances(Account account)
        {
            SharedOperationsService.AdjustTransactionBalances(account.Id, account);
        }

        public void AdjustAccountAmount(Account account)
        {
            SharedOperationsService.AdjustAccountAmount(account);
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