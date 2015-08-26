using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using FinancialPlannerApplication.Models;
using FinancialPlannerApplication.Models.DataAccess;
using FinancialPlannerApplication.Models.Services;
using FinancialPlannerApplication.Models.ViewModels;

namespace FinancialPlannerApplication.Controllers
{
    [Authorize]
    public class BudgetController : Controller
    {
        private readonly IFinancialPlannerRepository FinancialPlannerRepository;
        private readonly IBudgetService BudgetService;
        private readonly ISetViewModelsService SetViewModelsService;
        private const int NotSelected = -1;

        public BudgetController(IFinancialPlannerRepository financialPlannerRepository, IBudgetService budgetService,
            ISetViewModelsService setViewModelsService)
        {
            FinancialPlannerRepository = financialPlannerRepository;
            BudgetService = budgetService;
            SetViewModelsService = setViewModelsService;
        }

        public ActionResult Index(int budgetId = 0, int budgetItemId = 0)
        {
            var username = User.Identity.Name;
            var vm = BudgetService.MapBudgetIndexViewModel(username);

            vm.SelectedBudgetId = budgetId;
            vm.SelectedBudgetItemId = budgetItemId;
            
            return View(vm);
        }

        public ActionResult AddBudget()
        {
            ViewBag.Title = "Add Budget";

            return PartialView("EditBudget");
        }

        [HttpPost]
        public ActionResult AddBudget(EditBudgetViewModel vm, bool isDone = false)
        {
            ViewBag.Title = "Add Budget";

            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                return ReturnViewWithDateError(vm);
            }

            ViewBag.Title = "Edit Budget";

            return vm.Id == 0 ? AddBudgetReturnEditBudgetView(vm, isDone) : EditBudget(vm, isDone);
        }

        private ActionResult AddBudgetReturnEditBudgetView(EditBudgetViewModel vm, bool isDone = false)
        {
            var username = User.Identity.Name;
            var budget = BudgetService.AddBudget(vm, username);

            ModelState.Clear();
            vm.Id = budget.Id;

            if (isDone)
                return RedirectToAction("Index", new {budgetId = budget.Id});

            return PartialView("EditBudget", vm);
        }

        private ActionResult ReturnViewWithDateError(EditBudgetViewModel vm)
        {
            if (vm.EndDate < vm.StartDate)
            {
                ModelState.AddModelError("StartEnd", "End Date must be later than Start Date");
            }

            return PartialView("EditBudget", vm);
        }

        [HttpPost]
        public ActionResult EditBudget(EditBudgetViewModel vm, bool isDone = false)
        {
            ViewBag.Title = "Edit Budget";

            if (!ModelState.IsValid)
                return ReturnViewWithDateError(vm);             

            var saved = BudgetService.EditBudget(vm);

            if (!saved)
                return PartialView("Error");

            if (isDone)
                return RedirectToAction("Index", new {budgetId = vm.Id});

            return PartialView("EditBudget", vm);
        }

        public ActionResult AddBudgetItem()
        {
            var username = User.Identity.Name;
            var vm = BudgetService.MapEditBudgetItemViewModelForAdd(username);

            ViewBag.Title = "Add Budget Item";

            return PartialView("EditBudgetItem", vm);
        }

        [HttpPost]
        public ActionResult AddBudgetItem(EditBudgetItemViewModel vm, bool isDone)
        {
            ViewBag.Title = "Add Budget Item";

            var budgets = FinancialPlannerRepository.GetBudgets().Where(m => m.Username == User.Identity.Name);
            vm.Budgets = SetViewModelsService.SetBudgetViewModels(budgets);

            if (vm.SelectedBudgetId == NotSelected)
                ModelState.AddModelError("SelectedBudgetId", "Budget must be selected");

            if (!ModelState.IsValid)
                return PartialView("EditBudgetItem", vm);

            ViewBag.Title = "Edit Budget Item";

            var budgetItem = new BudgetItem();
            if (vm.BudgetItemId == 0)
            {
                budgetItem = BudgetService.AddBudgetItem(vm);
                ModelState.Remove("BudgetItemId");
                vm.BudgetItemId = budgetItem.Id;

                if (isDone)
                    return RedirectToAction("Index", new {budgetId = budgetItem.BudgetId, budgetItemId = budgetItem.Id});

                return PartialView("EditBudgetItem", vm);
            }

            return isDone ? RedirectToAction("Index", new {budgetId = budgetItem.BudgetId, budgetItemId = budgetItem.Id}) : EditBudgetItem(vm, false);
        }

        [HttpPost]
        public ActionResult EditBudgetItem(EditBudgetItemViewModel vm, bool isDone)
        {
            ViewBag.Title = "Edit Budget Item";

            var budgets = FinancialPlannerRepository.GetBudgets().Where(m => m.Username == User.Identity.Name);
            vm.Budgets = SetViewModelsService.SetBudgetViewModels(budgets);

            if (vm.SelectedBudgetId == NotSelected)
            {
                ModelState.AddModelError("SelectedBudgetId", "Budget must be selected");
                return PartialView("EditBudgetItem", vm);
            }

            var saved = BudgetService.EditBudgetItem(vm);

            if (!saved)
                return View("Error");

            if (isDone)
                return RedirectToAction("Index", new { budgetId = vm.SelectedBudgetId, budgetItemId = vm.BudgetItemId });

            return PartialView("EditBudgetItem", vm);
        }

        public ActionResult AddBudgetItemTransaction(int id)
        {
            var username = User.Identity.Name;

            var vm = BudgetService.MapEditBudgetItemTransactionViewModelForAdd(username, id);

            if (vm == null)
                return View("Error");

            ViewBag.Title = "Add Budget Item Transaction";

            return PartialView("EditBudgetItemTransaction", vm);
        }

        public ActionResult LoadBudgetDetails(int id)
        {
            var vm = BudgetService.MapBudgetDetailsViewModel(id);

            return PartialView("BudgetDetails", vm);
        }

        public ActionResult LoadBudgetItemTransactions(int id)
        {
            var vm = BudgetService.MapBudgetItemTransactionViewModel(id);

            return PartialView("BudgetItemTransactions", vm);
        }

        public ActionResult EditBudget(int id)
        {
            var vm = BudgetService.MapEditBudgetViewModel(id);

            if (vm == null)
                return View("Error");

            ViewBag.Title = "Edit Budget";

            return PartialView(vm);
        }

        public ActionResult EditBudgetItem(int id)
        {
            var username = User.Identity.Name;
            var vm = BudgetService.MapEditBudgetItemViewModel(id, username);

            if (vm == null)
                return View("Error");

            ViewBag.Title = "Edit Budget Item";

            return PartialView(vm);
        }

        public ActionResult EditBudgetItemTransaction(int id)
        {
            var username = User.Identity.Name;

            var vm = BudgetService.MapEditBudgetItemTransactionViewModelForEdit(id, username);

            if (vm == null)
                return View("Error");

            ViewBag.Title = "Edit Budget Item Transaction";

            return PartialView(vm);
        }

        [HttpPost]
        public ActionResult AddBudgetItemTransaction(EditBudgetItemTransactionViewModel vm, bool isDone)
        {
            ViewBag.Title = "Add Budget Item Transaction";
            var username = User.Identity.Name;

            if (vm.FindingBudgetItems)
            {
                return FindBudgetItemsReturnEditTransactionView(ref vm, username);
            }

            if (vm.BudgetItemTransactionId == 0)
            {
                if (vm.SelectedBudgetId == NotSelected)
                {
                    ModelState.AddModelError("SelectedBudgetId", "A budget must be selected");
                }

                if (!ModelState.IsValid)
                {
                    vm = BudgetService.MapEditBudgetItemTransactionViewModelChangeBudget(username, vm);
                    return View("EditBudgetItemTransaction", vm);
                }

                ViewBag.Title = "Edit Budget Item Transaction";

                var budgetItemTransaction = BudgetService.AddBudgetItemTransaction(vm);

                ModelState.Remove("BudgetItemTransactionId");

                vm.BudgetItemTransactionId = budgetItemTransaction.Id;

                var budgetItemSaved = BudgetService.EditBudgetItemForAdd(vm);

                if (!budgetItemSaved)
                    return View("Error");

                if (vm.SelectedAccountId != -1)
                {
                    var account = FinancialPlannerRepository.GetAccounts().FirstOrDefault(m => m.Id == vm.SelectedAccountId);

                    if (account == null)
                        return View("Error");

                    EditAccountAmountAndTransactionBalances(vm, account);
                }

                EditExpenseBalance(vm, budgetItemTransaction);

                vm = BudgetService.MapEditBudgetItemTransactionViewModelChangeBudget(username, vm);

                if (isDone)
                    return RedirectToAction("Index",
                        new {budgetId = vm.SelectedBudgetId, budgetItemId = vm.SelectedBudgetItemId});

                return PartialView("EditBudgetItemTransaction", vm);
            }

            ViewBag.Title = "Edit Budget Transaction";

            return isDone ? RedirectToAction("Index", new { budgetId = vm.SelectedBudgetId, budgetItemId = vm.SelectedBudgetItemId }) : EditBudgetItemTransaction(vm, false);
        }

        private void EditExpenseBalance(EditBudgetItemTransactionViewModel vm, Transaction budgetItemTransaction)
        {
            var expense = FinancialPlannerRepository.GetExpenses().FirstOrDefault(m => m.Id == vm.SelectedExpenseId);

            if (expense != null)
            {
                BudgetService.AdjustExpenseBalance(expense, budgetItemTransaction);
            }
        }

        private void EditAccountAmountAndTransactionBalances(EditBudgetItemTransactionViewModel vm, Account account)
        {
            var transactions =
                FinancialPlannerRepository.GetTransactions()
                    .Where(m => m.AccountId == vm.SelectedAccountId)
                    .ToList();

            BudgetService.EditAccountAmount(vm, account, transactions);

            BudgetService.EditAccountTransactionBalances(vm, account, transactions);
        }

        private ActionResult FindBudgetItemsReturnEditTransactionView(ref EditBudgetItemTransactionViewModel vm, string username)
        {
            vm = BudgetService.MapEditBudgetItemTransactionViewModelChangeBudget(username, vm);
            vm.FindingBudgetItems = false;

            ModelState.Clear();

            return PartialView("EditBudgetItemTransaction", vm);
        }

        [HttpPost]
        public ActionResult EditBudgetItemTransaction(EditBudgetItemTransactionViewModel vm, bool isDone)
        {
            @ViewBag.Title = "Edit Budget Transaction";

            if (vm.FindingBudgetItems)
            {
                return FindBudgetItemsAndReturnEditTransactionView(ref vm);
            }

            var username = User.Identity.Name;

            if (vm.SelectedBudgetItemId == NotSelected)
            {
                ModelState.AddModelError("SelectedBudgetItemId", "Must select a budget item");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Edit Budget Transaction";
                vm = BudgetService.MapEditBudgetItemTransactionViewModelChangeBudget(username, vm);
                return PartialView("EditBudgetItemTransaction", vm);
            }

            var budgetItemTransaction =
                FinancialPlannerRepository.GetTransactions().FirstOrDefault(m => m.Id == vm.BudgetItemTransactionId);

            if (budgetItemTransaction == null || !ModelState.IsValid)
                return View("Error");

            var oldBudgetItemId = budgetItemTransaction.BudgetItemId;
            var wasWithdrawal = budgetItemTransaction.IsWithdrawal;
            var oldTransactionAmount = budgetItemTransaction.Amount;
            var oldExpenseId = budgetItemTransaction.ExpenseId;

            BudgetService.EditBudgetTransaction(budgetItemTransaction, vm);

            if (vm.SelectedAccountId != -1)
            {
                var account = FinancialPlannerRepository.GetAccounts()
                   .FirstOrDefault(m => m.Id == vm.SelectedAccountId);

                if (account == null)
                    return View("Error");

                EditAccountAmountAndTransactionBalances(vm, account);
            }

            if (vm.OldAccountId != null && vm.OldAccountId != 0)
            {
                var originalAccount = FinancialPlannerRepository.GetAccounts().FirstOrDefault(m => m.Id == vm.OldAccountId);

                if (originalAccount == null)
                    return View("Error");

                EditAccountAmountAndTransactionBalances(vm, originalAccount);
            }

            EditBudgetItemBalances(vm, oldBudgetItemId, budgetItemTransaction, oldTransactionAmount, wasWithdrawal);

            AdjustExpenseBalances(vm, oldExpenseId);

            vm = BudgetService.MapEditBudgetItemTransactionViewModelChangeBudget(username, vm);

            if (isDone)
                return RedirectToAction("Index",
                    new {budgetId = vm.SelectedBudgetId, budgetItemId = vm.SelectedBudgetItemId});

            return PartialView("EditBudgetItemTransaction", vm);
        }

        private void AdjustExpenseBalances(EditBudgetItemTransactionViewModel vm, int? oldExpenseId)
        {
            var expense = FinancialPlannerRepository.GetExpenses().FirstOrDefault(m => m.Id == vm.SelectedExpenseId);

            if (expense != null)
            {
                BudgetService.AdjustExpenseBalance(expense);
            }

            var oldExpense = FinancialPlannerRepository.GetExpenses().FirstOrDefault(m => m.Id == oldExpenseId);

            if (oldExpense != null)
            {
                BudgetService.AdjustExpenseBalance(oldExpense);
            }
        }

        private void EditBudgetItemBalances(EditBudgetItemTransactionViewModel vm, int? oldBudgetItemId,
            Transaction budgetItemTransaction, decimal oldTransactionAmount, bool wasWithdrawal)
        {
            var oldBudgetItem =
                FinancialPlannerRepository.GetBudgetItems().FirstOrDefault(m => m.Id == oldBudgetItemId);
            var budgetItem =
                FinancialPlannerRepository.GetBudgetItems()
                    .FirstOrDefault(m => m.Id == budgetItemTransaction.BudgetItemId);

            if (oldBudgetItem != null)
            {
                BudgetService.EditBudgetItemBalance(oldBudgetItem);
            }

            if (budgetItem != null)
            {
                BudgetService.EditBudgetItemBalance(budgetItem);
            }
        }

        private ActionResult FindBudgetItemsAndReturnEditTransactionView(ref EditBudgetItemTransactionViewModel vm)
        {
            vm = BudgetService.MapEditBudgetItemTransactionViewModelForChange(vm,
                User.Identity.Name);

            ModelState.Remove("FindingBudgetItems");
            vm.FindingBudgetItems = false;

            return PartialView("EditBudgetItemTransaction", vm);
        }
    }
}