using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;

namespace BudgetManagement.Models
{
    public class CreateCountViewModel:Count
    {
        public IEnumerable<SelectListItem>? CountTypes { get; set; }
    }
}
