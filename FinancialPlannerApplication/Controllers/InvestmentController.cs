using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinancialPlannerApplication.Controllers
{
    [Authorize]
    public class InvestmentController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}