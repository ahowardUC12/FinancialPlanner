using System.ComponentModel.DataAnnotations;

namespace FinancialPlannerApplication.Models.ViewModels
{
    public class EditAccountViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
        public string UserName { get; set; }
    }
}