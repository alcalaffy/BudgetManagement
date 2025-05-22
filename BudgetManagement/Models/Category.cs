using System.ComponentModel.DataAnnotations;

namespace BudgetManagement.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(maximumLength: 50, ErrorMessage = "Cant' be mora than {1} characters")]
        [Display(Name = "Name")]
        public string Nombre { get; set; }
        [Display(Name ="Operation Type")]
        public OperationType TipoOperacionId { get; set; }
        public int UsuarioId { get; set; }
    }
}
