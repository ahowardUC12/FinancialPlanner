using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialPlannerApplication.Models.ViewModels
{
    public class EditBudgetViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }
    }
}