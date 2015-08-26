using System.Linq;
using System.Web.Mvc;
using FinancialPlannerApplication.Models;
using FinancialPlannerApplication.Models.DataAccess;
using FinancialPlannerApplication.Models.Services;
using FinancialPlannerApplication.Models.ViewModels;

namespace FinancialPlannerApplication.Controllers
{
    [Authorize]
    public class FinancialPlannerAccountController : Controller
    {
        private readonly IFinancialPlannerRepository FinancialPlannerRepository;
        private readonly IFinancialPlannerAccountService FinancialPlannerAccountService;
        private const int NotSelected = -1;

        public FinancialPlannerAccountController(IFinancialPlannerRepository financialPlannerRepository,
            IFinancialPlannerAccountService financialPlannerAccountService)
        {
            FinancialPlannerRepository = financialPlannerRepository;
            FinancialPlannerAccountService = financialPlannerAccountService;
        }

        public ActionResult Index(int accountId = 0)
        {
            var vm = FinancialPlannerAccountService.MapAccountIndexViewModel(User.Identity.Name);

            if(vm != null)
                vm.SelectedAccountId = accountId;

            return View(vm);
        }

        public ActionResult AddAccount()
        {
            return MapEditAccountViewModel();
        }

        [HttpPost]
        public ActionResult AddAccount(EditAccountViewModel vm, bool isDone = false)
        {
            ViewBag.Title = "Add Account";

            if (!ModelState.IsValid)
                return PartialView("EditAccount", vm);

            if (vm.Id == 0)
            {
                ModelState.Remove("Id");
                var account = FinancialPlannerAccountService.AddAccount(User.Identity.Name, vm);
                vm.Id = account.Id;

                ViewBag.Title = "Edit Account";

                if (isDone)
                    return RedirectToAction("Index", new {accountId = account.Id});

                return PartialView("EditAccount", vm);
            }

            return EditAccount(vm, isDone);
        }

        public ActionResult AddAccountTransaction()
        {
            var username = User.Identity.Name;
            var vm = FinancialPlannerAccountService.MapEditTransactionViewModel(username);

            ViewBag.Title = "Add Account Transaction";

            return PartialView("EditTransaction", vm);
        }

        public ActionResult EditAccount(int id)
        {
            return MapEditAccountViewModel(id);
        }

        [HttpPost]
        public ActionResult EditAccount(EditAccountViewModel vm, bool isDone = false)
        {
            var username = User.Identity.Name;

            var saved = FinancialPlannerAccountService.EditAccount(vm, username);

            if (!saved)
                return PartialView("Error");

            ViewBag.Title = "Edit Account";

            if (isDone)
                return RedirectToAction("Index", new {accountId = vm.Id});
            
            return PartialView("EditAccount", vm);
        }

        public ActionResult LoadAccountTransactions(int id)
        {
            var vm = FinancialPlannerAccountService.MapAccountTransactionsViewModel(id);

            return PartialView("AccountTransactions", vm);
        }

        public ActionResult EditTransaction(int id, int accountId)
        {
            var username = User.Identity.Name;
            var vm = FinancialPlannerAccountService.MapEditTransactionViewModelForEdit(id, accountId, username);

            if (vm == null)
                return PartialView("Error");

            ViewBag.Title = "Edit Transaction";

            return PartialView("EditTransaction", vm);
        }

        public ActionResult AddTransaction(int id)
        {
            var username = User.Identity.Name;
            var vm = FinancialPlannerAccountService.MapEditTransactionViewModelForAdd(id, username);

            if (vm == null)
                return PartialView("Error");

            ViewBag.Title = "Add Transaction";

            return PartialView("EditTransaction", vm);
        }

        [HttpPost]
        public ActionResult AddTransaction(EditTransactionViewModel vm, bool isDone = false)
        {
            ViewBag.Title = "Add Transaction";

            var transaction = new Transaction();

            if (vm.FindingBudgetItems)
            {
                return FindBudgetItemsAndReturn(vm);
            }

            AddNoAccountSelectedError(vm);

            if (!ModelState.IsValid)
            {
                return MapEditTransactionViewModelAndReturn(vm);
            }

            if (vm.AccountTransactionId == 0)
            {
                var account = FinancialPlannerRepository.GetAccounts().FirstOrDefault(m => m.Id == vm.SelectedAccountId);

                if (account == null)
                    return PartialView("Error");

                transaction = AdjustBalancesForTransaction(vm, account);

                ModelState.Remove("AccountTransactionId");
                FinancialPlannerAccountService.MapEditTransactionViewModel(vm, User.Identity.Name);
                vm.AccountTransactionId = transaction.Id;

                ViewBag.Title = "Edit Transaction";

                if (isDone)
                    return RedirectToAction("Index", new {accountId = transaction.AccountId});

                return PartialView("EditTransaction", vm);
            }

            return isDone ? RedirectToAction("Index", new { accountId = transaction.AccountId }) : EditTransaction(vm);
        }

        [HttpPost]
        public ActionResult AddTransactionDone(EditTransactionViewModel vm)
        {
            ViewBag.Title = "Add Transaction";

            var transaction = new Transaction();

            if (vm.FindingBudgetItems)
            {
                return FindBudgetItemsAndReturn(vm);
            }

            AddNoAccountSelectedError(vm);

            if (!ModelState.IsValid)
            {
                return MapEditTransactionViewModelAndReturn(vm);
            }

            if (vm.AccountTransactionId == 0)
            {
                var account = FinancialPlannerRepository.GetAccounts().FirstOrDefault(m => m.Id == vm.SelectedAccountId);

                if (account == null)
                    return PartialView("Error");

                transaction = AdjustBalancesForTransaction(vm, account);

                ModelState.Remove("AccountTransactionId");
                FinancialPlannerAccountService.MapEditTransactionViewModel(vm, User.Identity.Name);
                vm.AccountTransactionId = transaction.Id;

                ViewBag.Title = "Edit Transaction";
            }


            return RedirectToAction("Index", new{accountId = transaction.AccountId});
        }

        private Transaction AdjustBalancesForTransaction(EditTransactionViewModel vm, Account account)
        {
            FinancialPlannerAccountService.AdjustAccountBalance(account, vm);

            var transaction = FinancialPlannerAccountService.AddTransaction(account, vm);

            FinancialPlannerAccountService.AdjustTransactionBalances(account, vm);

            if (vm.SelectedBudgetItemId != null && vm.SelectedBudgetItemId > 0)
            {
                AdjustBudgetItemBalance(vm);
            }

            var expense = FinancialPlannerRepository.GetExpenses().FirstOrDefault(m => m.Id == vm.SelectedExpenseId);

            if (expense != null)
            {
                FinancialPlannerAccountService.AdjustExpenseBalance(expense, transaction);
            }
            return transaction;
        }

        private void AdjustBudgetItemBalance(EditTransactionViewModel vm)
        {
            var budgetItem =
                FinancialPlannerRepository.GetBudgetItems().FirstOrDefault(m => m.Id == vm.SelectedBudgetItemId);

            if (budgetItem != null)
                FinancialPlannerAccountService.AdjustBudgetItemBalance(budgetItem, vm);
        }

        private ActionResult MapEditTransactionViewModelAndReturn(EditTransactionViewModel vm)
        {
            FinancialPlannerAccountService.MapEditTransactionViewModel(vm, User.Identity.Name);

            return PartialView("EditTransaction", vm);
        }

        private void AddNoAccountSelectedError(EditTransactionViewModel vm)
        {
            if (vm.SelectedAccountId == NotSelected)
            {
                ModelState.AddModelError("SelectedAccountId", "An account must be selected");
            }
        }

        private ActionResult FindBudgetItemsAndReturn(EditTransactionViewModel vm)
        {
            FinancialPlannerAccountService.MapEditTransactionViewModel(vm, User.Identity.Name);

            ModelState.Remove("FindingBudgetItems");
            vm.FindingBudgetItems = false;

            return PartialView("EditTransaction", vm);
        }

        [HttpPost]
        public ActionResult EditTransaction(EditTransactionViewModel vm, bool isDone = false)
        {
            @ViewBag.Title = "Edit Transaction";

            if (vm.FindingBudgetItems)
            {
                return FindBudgetItemsAndReturn(vm);
            }

            AddNoAccountSelectedError(vm);

            if (!ModelState.IsValid)
            {
                return MapEditTransactionViewModelAndReturn(vm);
            }

            var account = FinancialPlannerRepository.GetAccounts().FirstOrDefault(m => m.Id == vm.SelectedAccountId);

            var transaction = FinancialPlannerRepository.GetTransactions().FirstOrDefault(m => m.Id == vm.AccountTransactionId);
            
            if (transaction == null)
                return PartialView("Error");

            var oldBudgetItemId = transaction.BudgetItemId;
            var oldExpenseId = transaction.ExpenseId;
            transaction.AccountId = vm.SelectedAccountId;

            FinancialPlannerAccountService.EditTransaction(vm);

            if(oldBudgetItemId != null)
                FinancialPlannerAccountService.AdjustOldBudgetItemBalance(oldBudgetItemId);

            if (oldBudgetItemId != vm.SelectedBudgetItemId && vm.SelectedBudgetItemId != null)
            {
                FinancialPlannerAccountService.AdjustNewBudgetItemBalance(vm);
            }

            AdjustAccountTransactionBalances(vm, account);

            AdjustExpenseBalances(vm, oldExpenseId);

            AdjustAccountAmounts(vm, account);

            @ViewBag.Title = "Edit Transaction";

            FinancialPlannerAccountService.MapEditTransactionViewModel(vm, User.Identity.Name);

            if(isDone)
                return RedirectToAction("Index", new { accountId = transaction.AccountId });
            else
                return PartialView("EditTransaction", vm);
        }

        [HttpPost]
        public ActionResult EditTransactionDone(EditTransactionViewModel vm)
        {
            @ViewBag.Title = "Edit Transaction";

            if (vm.FindingBudgetItems)
            {
                return FindBudgetItemsAndReturn(vm);
            }

            AddNoAccountSelectedError(vm);

            if (!ModelState.IsValid)
            {
                return MapEditTransactionViewModelAndReturn(vm);
            }

            var account = FinancialPlannerRepository.GetAccounts().FirstOrDefault(m => m.Id == vm.SelectedAccountId);

            var transaction = FinancialPlannerRepository.GetTransactions().FirstOrDefault(m => m.Id == vm.AccountTransactionId);

            if (transaction == null)
                return PartialView("Error");

            var oldBudgetItemId = transaction.BudgetItemId;
            var oldExpenseId = transaction.ExpenseId;
            transaction.AccountId = vm.SelectedAccountId;

            FinancialPlannerAccountService.EditTransaction(vm);

            if(oldBudgetItemId != null)
                FinancialPlannerAccountService.AdjustOldBudgetItemBalance(oldBudgetItemId);

            if (oldBudgetItemId != vm.SelectedBudgetItemId && vm.SelectedBudgetItemId != null)
            {
                FinancialPlannerAccountService.AdjustNewBudgetItemBalance(vm);
            }

            AdjustAccountTransactionBalances(vm, account);

            AdjustExpenseBalances(vm, oldExpenseId);

            AdjustAccountAmounts(vm, account);

            @ViewBag.Title = "Edit Transaction";

            FinancialPlannerAccountService.MapEditTransactionViewModel(vm, User.Identity.Name);

            return RedirectToAction( "Index", new {accountId = transaction.AccountId});
        }


        private void AdjustAccountAmounts(EditTransactionViewModel vm, Account account)
        {
            FinancialPlannerAccountService.AdjustAccountAmount(account);

            if (vm.OldAccountId != vm.SelectedAccountId && vm.OldAccountId != 0)
            {
                var oldAccount = FinancialPlannerRepository.GetAccounts().FirstOrDefault(m => m.Id == vm.OldAccountId);

                if (oldAccount != null)
                {
                    FinancialPlannerAccountService.AdjustAccountAmount(oldAccount);
                }
            }
        }

        private void AdjustExpenseBalances(EditTransactionViewModel vm, int? oldExpenseId)
        {
            var expense = FinancialPlannerRepository.GetExpenses().FirstOrDefault(m => m.Id == vm.SelectedExpenseId);

            if (expense != null)
            {
                FinancialPlannerAccountService.AdjustExpenseBalance(expense);
            }

            var oldExpense = FinancialPlannerRepository.GetExpenses().FirstOrDefault(m => m.Id == oldExpenseId);

            if (oldExpense != null)
            {
                FinancialPlannerAccountService.AdjustExpenseBalance(oldExpense);
            }
        }

        private void AdjustAccountTransactionBalances(EditTransactionViewModel vm, Account account)
        {
            var originalAccount = FinancialPlannerRepository.GetAccounts().FirstOrDefault(m => m.Id == vm.OldAccountId);

            FinancialPlannerAccountService.AdjustTransactionBalances(account);

            if (originalAccount != null && account != null && (account.Amount != originalAccount.Amount))
            {
                var oldAccount = FinancialPlannerRepository.GetAccounts().FirstOrDefault(m => m.Id == vm.OldAccountId);

                if (oldAccount != null && account.Id != oldAccount.Id)
                {
                    FinancialPlannerAccountService.AdjustTransactionBalances(oldAccount);
                }
            }
        }

        private ActionResult MapEditAccountViewModel(int id = 0)
        {
            EditAccountViewModel vm;

            if (id != 0)
            {
                ViewBag.Title = "Edit Account";

                vm = FinancialPlannerAccountService.MapEditAccountViewModel(id);

                if(vm == null)
                    return PartialView("Error");
            }
            else
            {
                ViewBag.Title = "Add Account";

                vm = new EditAccountViewModel();
            }

            return PartialView("EditAccount", vm);
        }
    }
}