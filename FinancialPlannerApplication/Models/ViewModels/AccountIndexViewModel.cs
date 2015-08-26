using System.Collections.Generic;

namespace FinancialPlannerApplication.Models.ViewModels
{
    public class AccountIndexViewModel
    {
        public IList<AccountViewModel> Accounts { get; set; }
        public int SelectedAccountId { get; set; }
    }
}