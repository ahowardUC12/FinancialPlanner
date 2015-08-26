using System.ComponentModel.DataAnnotations;

namespace FinancialPlannerApplication.Models.ViewModels
{
    public class EditExpenseViewModel
    {
        public int ExpenseId { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [Display(Name = "Amount")]
        [Required(ErrorMessage = "Name is required.")]
        public decimal Amount { get; set; }
        [Display(Name = "Interest Rate")]
        public decimal InterestRate { get; set; }
    }
}