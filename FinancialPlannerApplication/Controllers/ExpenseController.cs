using System.Linq;
using System.Web.Mvc;
using FinancialPlannerApplication.Models;
using FinancialPlannerApplication.Models.DataAccess;
using FinancialPlannerApplication.Models.Services;
using FinancialPlannerApplication.Models.ViewModels;

namespace FinancialPlannerApplication.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private readonly IFinancialPlannerRepository FinancialPlannerRepository;
        private readonly IExpenseService ExpenseService;
        private const int NotSelected = -1;

        public ExpenseController(IFinancialPlannerRepository financialPlannerRepository,
            IExpenseService expenseService)
        {
            FinancialPlannerRepository = financialPlannerRepository;
            ExpenseService = expenseService;
        }

        public ActionResult Index(int expenseId = 0)
        {
            var username = User.Identity.Name;
            var vm = ExpenseService.MapExpenseIndexViewModel(username);

            vm.SelectedExpenseId = expenseId;

            return View(vm);
        }

        public ActionResult LoadExpenseTransactions(int id)
        {
            var vm = ExpenseService.MapExpenseTransactionsViewModel(id);

            return PartialView("Transactions", vm);
        }

        public ActionResult AddExpense()
        {
            ViewBag.Title = "Add Expense";

            return PartialView("EditExpense");
        }

        [HttpPost]
        public ActionResult AddExpense(EditExpenseViewModel vm, bool isDone = false)
        {
            ViewBag.Title = "Edit Expense";

            if (vm.ExpenseId == 0)
            {
                ModelState.Remove("ExpenseId");

                if (!ModelState.IsValid)
                {
                    ViewBag.Title = "Add Expense";
                    return PartialView("EditExpense");
                }

                var expense = ExpenseService.AddExpense(vm, User.Identity.Name);
                vm.ExpenseId = expense.Id;

                if (isDone)
                    return RedirectToAction("Index", new {expenseId = vm.ExpenseId});

                return PartialView("EditExpense", vm);
            }

            return isDone ? RedirectToAction("Index", new { expenseId = vm.ExpenseId }) : EditExpense(vm);
        }

        public ActionResult EditExpense(int id)
        {
            var vm = ExpenseService.MapEditExpenseViewModel(id);

            if (vm == null)
                return PartialView("Error");

            ViewBag.Title = "Edit Expense";

            return PartialView(vm);
        }

        [HttpPost]
        public ActionResult EditExpense(EditExpenseViewModel vm, bool isDone = false)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Edit Expense";
                return View(vm);
            }

            var saved = ExpenseService.EditExpense(vm);

            if (!saved)
                return PartialView("Error");

            if (isDone)
                return RedirectToAction("Index", new { expenseId = vm.ExpenseId });

            return PartialView("EditExpense", vm);
        }

        public ActionResult EditTransaction(int id, int expenseId)
        {
            var vm = ExpenseService.MapEditTransactionViewModelForEdit(id, expenseId, User.Identity.Name);

            if (vm == null)
                return PartialView("Error");

            ViewBag.Title = "Edit Transaction";

            return PartialView("EditTransaction", vm);
        }

        public ActionResult AddTransaction(int id)
        {
            var vm = ExpenseService.MapEditTransactionViewModelForAdd(id, User.Identity.Name);

            if (vm == null)
                return PartialView("Error");

            ViewBag.Title = "Add Transaction";

            return PartialView("EditTransaction", vm);
        }

        [HttpPost]
        public ActionResult AddTransaction(EditExpenseTransactionViewModel vm, bool isDone = false)
        {
            ViewBag.Title = "Add Transaction";
            
            var transaction = new Transaction();

            if (vm.FindingBudgetItems)
            {
                return FindBudgetItemsAndReturn(vm);
            }

            if (vm.SelectedExpenseId == NotSelected)
            {
                ModelState.AddModelError("SelectedExpenseId", "An expense must be selected");
            }

            if (!ModelState.IsValid)
            {
                ExpenseService.MapEditTransactionViewModel(vm, User.Identity.Name);

                return PartialView("EditTransaction", vm);
            }

            if (vm.ExpenseTransactionId == 0)
            {
                AdjustAccountAndTransactionBalances(vm);

                if (vm.SelectedBudgetItemId != null && vm.SelectedBudgetItemId > 0)
                {
                    AdjustBudgetItemBalance(vm);
                }

                var expense = FinancialPlannerRepository.GetExpenses().FirstOrDefault(m => m.Id == vm.SelectedExpenseId);

                transaction = ExpenseService.AddTransaction(expense, vm);
                ModelState.Remove("ExpenseTransactionId");
                vm.ExpenseTransactionId = transaction.Id;                

                if (expense != null)
                {
                    ExpenseService.AdjustExpenseBalance(expense, transaction);
                }

                ExpenseService.MapEditTransactionViewModel(vm, User.Identity.Name);

                ViewBag.Title = "Edit Transaction";

                if (isDone)
                    return RedirectToAction("Index", new { expenseId = transaction.ExpenseId });

                return PartialView("EditTransaction", vm);
            }

            return isDone ? RedirectToAction("Index", new { expenseId = transaction.ExpenseId }) : EditTransaction(vm);
        }

        private void AdjustBudgetItemBalance(EditExpenseTransactionViewModel vm)
        {
            var budgetItem =
                FinancialPlannerRepository.GetBudgetItems().FirstOrDefault(m => m.Id == vm.SelectedBudgetItemId);

            if (budgetItem != null)
                ExpenseService.AdjustBudgetItemBalance(budgetItem, vm);
        }

        private void AdjustAccountAndTransactionBalances(EditExpenseTransactionViewModel vm)
        {
            var account = FinancialPlannerRepository.GetAccounts().FirstOrDefault(m => m.Id == vm.SelectedAccountId);

            if (account != null)
            {
                ExpenseService.AdjustAccountBalance(account, vm);
                ExpenseService.AdjustTransactionBalances(account, vm);
            }
        }

        private ActionResult FindBudgetItemsAndReturn(EditExpenseTransactionViewModel vm)
        {
            ExpenseService.MapEditTransactionViewModel(vm, User.Identity.Name);

            ModelState.Remove("FindingBudgetItems");
            vm.FindingBudgetItems = false;

            return PartialView("EditTransaction", vm);
        }

        [HttpPost]
        public ActionResult EditTransaction(EditExpenseTransactionViewModel vm, bool isDone = false)
        {
            @ViewBag.Title = "Edit Transaction";

            if (vm.FindingBudgetItems)
            {
                return FindBudgetItemsAndReturn(vm);
            }

            if (vm.SelectedExpenseId == NotSelected)
            {
                ModelState.AddModelError("SelectedExpenseId", "An expense must be selected");
            }

            if (!ModelState.IsValid)
            {
                ExpenseService.MapEditTransactionViewModel(vm, User.Identity.Name);

                return PartialView("EditTransaction", vm);
            }

            var transaction = FinancialPlannerRepository.GetTransactions().FirstOrDefault(m => m.Id == vm.ExpenseTransactionId);

            if (transaction == null)
                return PartialView("Error");

            ExpenseService.EditTransaction(vm);

            var oldBudgetItemId = transaction.BudgetItemId;
            var oldExpenseId = transaction.ExpenseId;
            var oldAccountId = transaction.AccountId;

            transaction.AccountId = vm.SelectedAccountId;

            AdjustBudgetItemBalances(vm, oldBudgetItemId);
            AdjustAccountAmountsAndBalances(vm, oldAccountId);

            var expense = FinancialPlannerRepository.GetExpenses().FirstOrDefault(m => m.Id == vm.SelectedExpenseId);

            if (expense == null)
                return PartialView("Error");

            AdjustExpenseBalances(expense, oldExpenseId);

            ExpenseService.MapEditTransactionViewModel(vm, User.Identity.Name);

            if (isDone)
                return RedirectToAction("Index", new {expenseId = expense.Id});

            return PartialView("EditTransaction", vm);
        }

        private void AdjustExpenseBalances(Expense expense, int? oldExpenseId)
        {
            ExpenseService.AdjustExpenseBalance(expense);

            var oldExpense = FinancialPlannerRepository.GetExpenses().FirstOrDefault(m => m.Id == oldExpenseId);

            if (oldExpense != null && (expense.Id != oldExpense.Id))
            {
                ExpenseService.AdjustExpenseBalance(oldExpense);
            }
        }

        private void AdjustAccountAmountsAndBalances(EditExpenseTransactionViewModel vm, int? oldAccountId)
        {
            var account = FinancialPlannerRepository.GetAccounts().FirstOrDefault(m => m.Id == vm.SelectedAccountId);
            var originalAccount = FinancialPlannerRepository.GetAccounts().FirstOrDefault(m => m.Id == oldAccountId);

            if (account != null)
            {
                ExpenseService.AdjustTransactionBalances(account);
                ExpenseService.AdjustAccountAmount(account);
            }

            if (originalAccount != null && account != null && (account.Amount != originalAccount.Amount))
            {
                var oldAccount = FinancialPlannerRepository.GetAccounts().FirstOrDefault(m => m.Id == vm.OldExpenseId);

                if (oldAccount != null && account.Id != oldAccount.Id)
                {
                    ExpenseService.AdjustTransactionBalances(oldAccount);
                }
            }

            if (originalAccount != null && (originalAccount.Id != vm.SelectedAccountId && originalAccount.Id != 0))
            {
                ExpenseService.AdjustAccountAmount(originalAccount);
            }
        }

        private void AdjustBudgetItemBalances(EditExpenseTransactionViewModel vm, int? oldBudgetItemId)
        {
            ExpenseService.AdjustOldBudgetItemBalance(oldBudgetItemId);

            if (oldBudgetItemId != vm.SelectedBudgetItemId)
            {
                ExpenseService.AdjustNewBudgetItemBalance(vm);
            }
        }
    }
}