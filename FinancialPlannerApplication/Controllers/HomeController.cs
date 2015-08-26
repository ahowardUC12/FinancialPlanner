using System;
using System.Linq;
using System.Web.Mvc;
using FinancialPlannerApplication.Models.Services;

namespace FinancialPlannerApplication.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHomeService HomeService;

        public HomeController(IHomeService homeService)
        {
            HomeService = homeService;
        }

        public ActionResult Index()
        {
            var username = User.Identity.Name;
            var vm = HomeService.MapHomeIndexViewModel(username);
           
            return View(vm);
        }

        public JsonResult GetBudgetCharts(int budgetId = 0)
        {
            var username = User.Identity.Name;
            var budgetItemTotals = HomeService.GetBudgetItemTotals(username, budgetId);           

            return Json(budgetItemTotals, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBudgetItemCharts(int budgetId = 0)
        {
            var username = User.Identity.Name;
            var budgetTotalsVm = HomeService.GetBudgetTotalsViewModel(username, budgetId);

            return Json(budgetTotalsVm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBudgetProgress(int budgetId = 0)
        {
            var username = User.Identity.Name;
            var budgetProgress = HomeService.GetBudgetProgress(username, budgetId);

            return Json(budgetProgress, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccountCharts(int accountId, DateTime? fromDate, DateTime? toDate)
        {
            var transactionTotalsByDate = HomeService.GetAccountProgressViewModels(accountId, fromDate, toDate);

            return Json(transactionTotalsByDate, JsonRequestBehavior.AllowGet);
        }
    }
}