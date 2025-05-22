using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BudgetManagement.Models
{
    public class CreateTransactionViewModel:Transaction
    {
        [Display(Name = "Counts")]
        public IEnumerable<SelectListItem>? Cuentas { get; set; }
        [Display(Name = "Categories")]
        public IEnumerable<SelectListItem>? Categorias { get; set; }
        [Display(Name = "Operation Type")]
        public OperationType OperationTypeId { get; set; } = OperationType.Income;
    }
}
