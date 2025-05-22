using BudgetManagement.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace BudgetManagement.Models
{
    public class Count
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="The field {0} must be fill")]
        [StringLength(50)]
        [FirstLetterInUpperCase]
        [Display(Name = "Name")]
        public string? Nombre { get; set; }
        [Display(Name ="Count Type")]
        public int TipoCuentaId { get; set; }
        public decimal Balance { get; set; }
        [StringLength(1000)]
        [Display(Name = "Description")]
        public string? Descripcion { get; set; }
        public string? TipoCuenta { get; set; }
    }
}
