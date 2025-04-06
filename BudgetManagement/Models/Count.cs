using BudgetManagement.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace BudgetManagement.Models
{
    public class Count
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo {0} es obligatorio")]
        [StringLength(50)]
        [FirstLetterInUpperCase]
        public string? Nombre { get; set; }
        [Display(Name ="Count Type")]
        public int TipoCuentaId { get; set; }
        public decimal Balance { get; set; }
        [StringLength(1000)]
        public string? Descripcion { get; set; }
        public string? TipoCuenta { get; set; }
    }
}
